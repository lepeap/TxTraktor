using System.Collections.Generic;
using System.Linq;
using TxTraktor.Extension;
using TxTraktor.Source.Model;
using TxTraktor.Tokenize;

namespace TxTraktor.Compile
{
    internal class VariablesProcessor
    {
        private readonly string[] _lemmaKeys = {"lemma", "лемма"};
        private readonly string[] _regexKeys = {"regex", "рег"};
        private readonly Dictionary<string, IExtension> _extensions;
        private readonly ITokenizer _tokenizer;

        public VariablesProcessor(ITokenizer tokenizer, IEnumerable<IExtension> extensions)
        {
            _tokenizer = tokenizer;
            _extensions = extensions?.ToDictionary(x=>x.Name, x=>x);
        }
        
        public IEnumerable<Rule> Process(IEnumerable<Rule> rules, Dictionary<string, string> globalVariables)
        {
            foreach (var rule in rules)
            {
                if (string.IsNullOrWhiteSpace(rule.ExtensionQuery))
                {
                    _processRule(rule, globalVariables);
                    yield return rule;
                    continue;
                }
                
                if (_extensions==null || !_extensions.ContainsKey(rule.ExtensionType))
                    throw new CfgCompileException(
                        $"Undefined extension type '{rule.ExtensionType}' for rule '{rule.Name}'"
                        );

                foreach (var genRule in _processExtensionRule(rule))
                    yield return genRule;
            }
            
        }

        private void _processRule(Rule rule, Dictionary<string, string> globalVariables)
        {
            foreach (var item in rule.Items)
            {
                if (item.Type==RuleItemType.VariableName && (globalVariables==null || !globalVariables.ContainsKey(item.Key)))
                    throw new CfgCompileException($"Undefined global variable '{item.Key}' in rule '{rule.Name}'");

                if (item.Type == RuleItemType.VariableName)
                {
                    var variableName = item.Key;
                    item.Type = RuleItemType.Terminal;
                    item.Key = globalVariables[variableName];
                    if (!item.HasLocalName)
                        item.LocalName = variableName;
                }
            }
        }

        private IEnumerable<Rule> _processExtensionRule(Rule rule)
        {
            var extension = _extensions[rule.ExtensionType];
            var extResults = extension.Process(rule.ExtensionQuery);
            var varIndex = 0;
            foreach (var varDic in extResults)
            {
                var newRule = rule.Copy();
                foreach (var item in newRule.Items)
                {
                    if (item.Type == RuleItemType.VariableName && !varDic.ContainsKey(item.Key))
                    {
                        var message = $"Extension query '{rule.ExtensionQuery}' type '{rule.ExtensionType}'. No variable name '{item.Key}' in result.";
                        throw new CfgCompileException(message);
                    }
                    if (item.Type == RuleItemType.VariableName)
                    {
                        var typeConditions = item.Conditions.Where(c => _lemmaKeys.Contains(c.Key) || _regexKeys.Contains(c.Key)).ToArray();
                        if (typeConditions.Length > 1)
                        {
                            var message = $"Reg item {item.Key} of rule '{rule.Name}' has multiple type values 'regex' ad 'lemma'.";
                            throw new CfgCompileException(message);
                        }

                        RuleItemType type;
                        if (typeConditions.Length != 0 && _lemmaKeys.Contains(typeConditions[0].Key))
                            type = RuleItemType.Lemma;
                        else if (typeConditions.Length != 0 && _regexKeys.Contains(typeConditions[0].Key))
                            type = RuleItemType.Regex;
                        else
                            type = RuleItemType.Terminal;
                        
                        if (typeConditions.Length != 0)
                            item.RemoveCondition(typeConditions[0]);

                        var variableName = item.Key;
                        var idKey = $"{item.Key}_id";
                        var value = varDic[variableName];
                        var tokenized = _tokenizer.Tokenize(value).ToArray();
                        if (tokenized.Length > 1)
                        {
                            string ruleName = $"{newRule.Name}_gen_{variableName}_{varIndex}";
                            var genRule = new Rule(ruleName, 
                                tokenized.Select(t=>new RuleItem(RuleItemType.Terminal, t.Text)
                            ));
                            if (type != RuleItemType.Terminal)
                                genRule.Items.First().Type = type;
                            yield return genRule;
                            
                            item.Key = ruleName;
                            item.Type = RuleItemType.NonTerminal;
                        }
                        else
                        {
                            item.Type = type;
                            item.Key = value;
                        }

                        if (!item.HasLocalName)
                            item.LocalName = variableName;
                        
                        if (varDic.ContainsKey(idKey))
                            item.SemanticId = varDic[idKey];
                    }
                }
                varIndex++;
                newRule.ExtensionQuery = null;
                newRule.ExtensionType = null;
                yield return newRule;
            }
        }
        
    }
}