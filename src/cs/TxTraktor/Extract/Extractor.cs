using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using TxTraktor.Morphology;
using TxTraktor.Parse;
using TxTraktor.Parse.Forest;
using TxTraktor.Source.Model.Extraction;
using TxTraktor.Tokenize;

namespace TxTraktor.Extract
{
    internal class Extractor : IExtractor
    {
        private readonly ITokenizer _tokenizer;
        private readonly IMorphAnalizer _morphAnalizer;
        private readonly IChartParser _parser;
        private readonly ExtractorSettings _settings;
        private readonly ILogger<IExtractor> _logger;

        public Extractor(ITokenizer tokenizer,
            IMorphAnalizer morphAnalizer,
            IChartParser parser,
            ExtractorSettings settings,
            ILogger<IExtractor> logger)
        {
            _tokenizer = tokenizer;
            _morphAnalizer = morphAnalizer;
            _parser = parser;
            _settings = settings;
            _logger = logger;
        }

        public IEnumerable<ExtractionDic> Parse(string text, params string[] rulesToExtract)
        {
            _logger?.LogDebug("Начало извлечения из текста: {Text}", text);
            var tokens = _tokenizer.Tokenize(text).ToArray();
            _morphAnalizer?.SetMorphInfo(tokens);
            var chart = _parser.Parse(tokens, rulesToExtract);
            var complShart = chart.GetOnyCompletedChart();
            var result = _processChart(complShart, text)
                                .OrderBy(x => x.StartPosition)
                                .ThenBy(x => x.Count)
                                .Where(x => rulesToExtract.Length == 0 || rulesToExtract.Contains(x.Name));
                                
            return result;
        }

        private IEnumerable<ExtractionDic> _processChart(Chart chart, string sourceText)
        {
            var items = _selectStates(chart.FlatStates)
                .OrderBy(s => s.StartColumnIndex)
                .OrderBy(s => s.EndColumnIndex);

            return items.Select(s => _extract(s.Node, sourceText));
        }

        private IEnumerable<State> _selectStates(IEnumerable<State> states)
        {
            states = states.Where(x =>  x.IsValid &&
                                        !x.IsSystemIntermediate &&
                                        x.HasTemplate &&
                                       (_settings.RulesToExtract == null ||
                                        _settings.RulesToExtract.Contains(x.Rule.Name)));
            if (_settings.SelectLongest)
            {
                var diapDic = states.GroupBy(st => st.Rule.Name)
                    .Select(gr => (key: gr.Key, items: gr.ToArray()));

                foreach (var gr in diapDic)
                {
                    foreach (var item in gr.items)
                    {
                        var longestItem = gr.items
                            .Where(st =>
                                !ReferenceEquals(st, item) &&
                                (st.StartColumnIndex < item.StartColumnIndex &&
                                 st.EndColumnIndex >= item.EndColumnIndex ||
                                 st.StartColumnIndex <= item.StartColumnIndex &&
                                 st.EndColumnIndex > item.EndColumnIndex))
                            .OrderBy(x => x.StartColumnIndex)
                            .OrderByDescending(x => x.EndColumnIndex)
                            .FirstOrDefault();

                        if (longestItem == null
                            || (longestItem.StartColumnIndex == item.StartColumnIndex &&
                                longestItem.EndColumnIndex == item.EndColumnIndex))
                            yield return item;
                    }
                }
            }
            else
            {
                foreach (var state in states)
                {
                    yield return state;
                }
            }
        }


        private ExtractionDic _extract(Node node, string sourceText)
        {
            Template template = node.Rule.Template;
            _createRefDics(node, sourceText, out var nameDic, out var numberDic);
            var text = _getNodeText(node, sourceText, out int startPosition);
            var resDic = new ExtractionDic(node.Rule.Name, text, startPosition);
            foreach (var item in template.Items)
            {
                var key = item.Name;
                var value = _getTemplateItemValue(item, nameDic, numberDic);
                if (value == null)
                    continue;

                resDic[key] = value;
            }

            return resDic;
        }

        private ExtractionValue _getTemplateItemValue(
            TemplateItemBase item,
            Dictionary<string, ExtractionValue> nameDic,
            Dictionary<int, ExtractionValue> numberDic)
        {
            switch (item.Type)
            {
                case TemplateValueType.String:
                    return new ExtractionValue(item.As<TemplateItem<string>>().Value, ValueType.String);

                case TemplateValueType.Bool:
                    return new ExtractionValue(item.As<TemplateItem<bool>>().Value, ValueType.Bool);

                case TemplateValueType.Integer:
                    return new ExtractionValue(item.As<TemplateItem<int>>().Value, ValueType.Int);

                case TemplateValueType.Float:
                    return new ExtractionValue(item.As<TemplateItem<float>>().Value, ValueType.Float);
                case TemplateValueType.NumberRef:
                    var refValInd = item.As<TemplateItem<(int, string)>>().Value;
                    if (!numberDic.ContainsKey(refValInd.Item1))
                        return null;
                    return _extractValueRef(numberDic[refValInd.Item1], refValInd.Item2);

                case TemplateValueType.NameRef:
                    var refValName = item.As<TemplateItem<(string, string)>>().Value;
                    if (!nameDic.ContainsKey(refValName.Item1))
                        return null;
                    return _extractValueRef(nameDic[refValName.Item1], refValName.Item2);

                case TemplateValueType.List:
                    var items = item.As<TemplateItem<TemplateItemBase[]>>().Value;
                    var resList = new List<ExtractionValue>();
                    foreach (var itemBase in items)
                    {
                        var resValue = _getTemplateItemValue(itemBase, nameDic, numberDic);
                        if (resValue == null)
                            continue;

                        if (resValue.Type == ValueType.List)
                        {
                            foreach (var insideValue in resValue.GetValue<List<ExtractionValue>>())
                            {
                                resList.Add(insideValue);
                            }
                        }
                        else
                        {
                            resList.Add(resValue);
                        }
                    }

                    return new ExtractionValue(resList, ValueType.List);


                default:
                    throw new NotImplementedException($"Unsopported type {item.Type}");
            }
        }

        private string _getNodeText(Node node, string text, out int startPosition)
        {
            node.GetPositions(out startPosition, out int endPosition);
            var length = endPosition - startPosition;
            return text.Substring(startPosition, length);
        }

        private ExtractionValue _extractValueRef(ExtractionValue value, string key)
        {
            if (value.Type == ValueType.Dictionary)
            {
                var dic = value.GetValue<ExtractionDic>();

                if (dic.Count == 1 && dic.ContainsKey("Value") && (key == null || key == "Value"))
                    return dic["Value"];

                if (key == null)
                    return value;

                if (!dic.ContainsKey(key))
                    return null;

                return dic[key];
            }

            return key == null ? value : null;
        }

        private void _createRefDics(Node node,
            string sourceText,
            out Dictionary<string, ExtractionValue> nameDic,
            out Dictionary<int, ExtractionValue> numberDic
        )
        {
            var rule = node.Rule;
            nameDic = new Dictionary<string, ExtractionValue>();
            if (rule.HasStaticVars)
                _setStaticVars(nameDic, rule.StaticVars);
            numberDic = new Dictionary<int, ExtractionValue>();

            foreach (var nodeBase in node.Children)
            {
                ExtractionValue value = _getNodeValue(nodeBase, sourceText);
                numberDic[nodeBase.Index] = value;
                if (!nodeBase.HasLocalName)
                    continue;
                
                if (!nameDic.ContainsKey(nodeBase.LocalName))
                {
                    nameDic[nodeBase.LocalName] = value;
                }
                else if (nameDic[nodeBase.LocalName].Type!=ValueType.List)
                {
                    var listValue = new List<ExtractionValue>();
                    listValue.Add(nameDic[nodeBase.LocalName]);
                    listValue.Add(value);
                    nameDic[nodeBase.LocalName] = new ExtractionValue(listValue, ValueType.List);
                }
                else
                {
                    nameDic[nodeBase.LocalName]
                            .GetValue<List<ExtractionValue>>()
                            .Add(value);
                }
            }
        }

        private ExtractionValue _getNodeValue(NodeBase nodeBase, string sourceText)
        {
            if (nodeBase.Type == NodeType.Leaf)
            {
                return new ExtractionValue(nodeBase.As<Leaf>().Token.Text, ValueType.String, nodeBase.SemanticId);
            }

            var subNode = nodeBase.As<Node>();
            if (subNode.IsPossibleList)
            {

                var list = new List<ExtractionValue>();
                foreach (var childBase in subNode.Children)
                {
                    var value = _getNodeValue(childBase, sourceText);
                    if (value.Type == ValueType.List)
                    {
                        list.AddRange(value.GetValue<List<ExtractionValue>>());
                    }
                    else
                    {
                        list.Add(value);
                    }
                }
                return new ExtractionValue(list, ValueType.List, nodeBase.SemanticId);
            }
            
            // Если есть шаблон, то извлекаем словарь по шаблону
            // иначе возвращаем текст данной ветки
            if (subNode.Rule.HasTemplate)
            {
                var dic = _extract(subNode, sourceText);
                
                // Если есть в словаре есть только одно значение и оно с ключем Value, то возвращаем его
                if (dic.Count == 1 && dic.ContainsKey("Value"))
                {
                    return dic["Value"];
                }
                
                return new ExtractionValue(dic, ValueType.Dictionary, nodeBase.SemanticId);
            }
            
            var text = _getNodeText(subNode, sourceText, out int _);
            return new ExtractionValue(text, ValueType.String, nodeBase.SemanticId);
        }


        private void _setStaticVars(Dictionary<string, ExtractionValue> nameDic, TemplateItemBase[] staticVars)
        {
            foreach (var staticVar in staticVars)
            {
                object value;
                ValueType type;
                switch (staticVar.Type)
                {
                    case TemplateValueType.String:
                        value = ((TemplateItem<string>) staticVar).Value;
                        type = ValueType.String;
                        break;
                    case TemplateValueType.Integer:
                        value = ((TemplateItem<int>) staticVar).Value;
                        type = ValueType.Int;
                        break;
                    case TemplateValueType.Float:
                        value = ((TemplateItem<float>) staticVar).Value;
                        type = ValueType.Float;
                        break;
                    case TemplateValueType.Bool:
                        value = ((TemplateItem<bool>) staticVar).Value;
                        type = ValueType.Bool;
                        break;

                    default:
                        throw new NotImplementedException($"Unsopported static var type {staticVar.Type}");
                }

                nameDic[staticVar.Name] = new ExtractionValue(value, type);
            }
        }
    }
}