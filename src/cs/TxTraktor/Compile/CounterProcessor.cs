using System;
using System.Collections.Generic;
using System.Linq;
using TxTraktor.Source.Model;
using TxTraktor.Source.Model.Extraction;

namespace TxTraktor.Compile
{
    internal class CounterProcessor
    {
        private int _termId = 0;
        private IDictionary<Tuple<string, Counter, RuleItemType, int?, int?>, string> _cache 
            = new Dictionary<Tuple<string, Counter, RuleItemType, int?, int?>, string>();
        


        public IEnumerable<Rule> Process(IEnumerable<Rule> rules)
        {
            var result = new List<Rule>();
            foreach (var rl in rules)
            {
                result.Add(rl);
                var newItems = new List<RuleItem>();
                foreach (var item in rl.Items)
                {
                    if (item.HasCounter)
                    {
                        _createRules(item, out var resultItem, out var resultRules);
                        newItems.Add(resultItem);
                        if (resultRules!=null)
                            result.AddRange(resultRules);
                    }
                    else
                        newItems.Add(item);

                }
                rl.SetNewItems(newItems);
            }
            return result;
        }

        private void _createRules(RuleItem item, out RuleItem resultItem, out IEnumerable<Rule> resultRules)
        {
            int? min = item.Counter == Counter.Number ? (int?)item.CounterValue.MinValue : null;
            int? max = item.Counter == Counter.Number ? (int?)item.CounterValue.MaxValue : null;
            var cacheKey = new Tuple<string, Counter, RuleItemType, int?, int?>(item.Key, item.Counter, item.Type, min, max);
            if (_cache.ContainsKey(cacheKey))
            {
                resultItem = new RuleItem(RuleItemType.NonTerminal, 
                                          _cache[cacheKey],
                                          conditions: item.Conditions,
                                          localName: item.LocalName);
                resultRules = null;
                return;
            }
            
            
            string key = item.Type == RuleItemType.NonTerminal ? item.Key : $"TERMINAL_{_termId++}";
            key = $"{key}_{_keyPrefix(item.Counter, min, max)}";
            resultItem = new RuleItem(RuleItemType.NonTerminal, 
                                      key,
                                      conditions: item.Conditions,
                                      localName: item.LocalName);
            item.LocalName = null;
            switch (item.Counter)
            {
                case Counter.Star:
                {
                    var starItem = new RuleItem(RuleItemType.NonTerminal, 
                                            key,
                                            conditions: item.Conditions);
                    resultRules = new[]
                    {
                        new Rule(key, new [] {item, starItem}, isPossibleList: true),
                        new Rule(key, new RuleItem[0] )
                    };
                    break;
                }
                case Counter.Plus:
                {
                    var starTerm = new RuleItem(item.Type, item.Key, Counter.Star);
                    _createRules(starTerm, out var rezStarTerm, out var rezStarRules);
                    var list = new List<Rule>()
                    {
                        new Rule(key, new[] {item, rezStarTerm}, isPossibleList: true)
                    };
                    list.AddRange(rezStarRules);
                    resultRules = list;
                    break;
                }
                case Counter.Question:
                {
                    resultRules = new[]
                    {
                        new Rule(key, 
                            new[]{item}, 
                            // Прокидываем шаблон исходного правила
                            template: new Template(new[]
                            {
                                new TemplateItem<(int, string)>("Value", (0, null), TemplateValueType.NumberRef)
                            }),
                            isSystemIntermediate: true
                            ),
                        new Rule(key, new RuleItem[]{}), 
                    };
                    break;
                }
                case Counter.Number:
                {
                    // TODO: Закэшировать промежуточные каунтеры

                    var list = new List<Rule>();
                    
                    if (!min.HasValue) 
                        throw new ArgumentException($"Min value is required CounterType {item.Type}");
                    
                    if (!max.HasValue) 
                        throw new ArgumentException($"Min value is required CounterType {item.Type}");

                    if (min.Value == 0)
                    {
                        list.Add(new Rule(key, new RuleItem[0]));
                        min = 1;
                    }

                    for (int i = min.Value; i <= max.Value; i++)
                        list.Add(new Rule(key, Enumerable.Range(min.Value, i).Select(x => item), isPossibleList: true));

                    resultRules = list;
                    break;
                }


                default:
                    throw new NotSupportedException($"CounterEnum type {item.Counter} is not supported");
            }

            _cache[cacheKey] = key;
            
        }

        private string _keyPrefix(Counter type, int? min=null, int? max=null)
        {
            switch (type)
            {
                case Counter.Star:
                    return "STAR";
                case Counter.Plus:
                    return "PLUS";
                case Counter.Question:
                    return "QUESTION";
                case Counter.Number:
                    if (!min.HasValue)
                        throw new ArgumentException($"Min value is required for counter type {type}");
                    
                    return max.HasValue ? $"COUNTER_{min}_{max}" : $"COUNTER_{min}";
                default:
                    throw new NotSupportedException($"Counter type '{type}' is not supported");
            }
        }

    }
}