using System.Collections.Generic;
using System.Linq;
using TxTraktor.Compile.Model;
using TxTraktor.Compile.Validation;
using TxTraktor.Extension;
using TxTraktor.Source.Model;
using TxTraktor.Tokenize;
using Rule = TxTraktor.Compile.Model.Rule;
using RuleSrc = TxTraktor.Source.Model.Rule;

namespace TxTraktor.Compile
{
    internal class GrammarCompiler : IGrammarCompiler

    {
        private readonly ConditionManager _condMan;
        private readonly ValidationManager _valMan;
        private readonly VariablesProcessor _variablesProcessor;
        public GrammarCompiler(ITokenizer tokenizer, IEnumerable<IExtension> extensions)
        {
            _condMan =  new ConditionManager();
            _valMan = new ValidationManager();
            _variablesProcessor = new VariablesProcessor(tokenizer, extensions);
        }
        public IEnumerable<Rule> Compile(IEnumerable<Grammar> grammars, Dictionary<string, string> variables=null)
        {
            var dic = new Dictionary<string, Grammar>();
            foreach (var gram in grammars)
            {
                if (dic.ContainsKey(gram.Name))
                {
                    var message = $"Duplicate grammar name '{gram.Name}'. Grammar keys '{dic[gram.Name].Key}' '{gram.Key}' ";
                    throw new CfgCompileException(message);
                }

                dic[gram.Name] = gram;
            }

            _processImports(dic);
            var rules = dic.SelectMany(x => x.Value.Rules);
            return _compileRules(rules, variables);
        }

        public IEnumerable<Rule> CompileRules(IEnumerable<RuleSrc> srcRls, Dictionary<string, string> variables=null)
        {
            return _compileRules(srcRls, variables);
        }


        private void _processImports(IDictionary<string, Grammar> grDic)
        {
            foreach (var kp in grDic)
            {
                var gr = kp.Value;
                var rplDic = new Dictionary<string, string>();

                if (gr.HasGrammarImports)
                {
                    foreach (var imp in gr.Imports)
                    {
                        if (!grDic.ContainsKey(imp.Source))
                        {
                            var message = $"No grammar '{imp.Source}' for import in grammar '{gr.Name}'";
                            throw new CfgCompileException(message);
                        }

                        if (imp.Name != null)
                        {
                            rplDic[imp.LocalName] = $"{imp.Source}.{imp.Name}";
                        }
                    }
                }

                foreach (var rl in gr.Rules)
                {
                    if (rplDic.ContainsKey(rl.Name))
                    {
                        var message = $"Rule name {rl.Name} duplicates import {rplDic[rl.Name]}";
                        throw new CfgCompileException(message);
                    }
                    
                    rl.Name = $"{gr.Name}.{rl.Name}";
                    foreach (var item in rl.Items)
                        if (item.HasFullKey) { }
                        else if (item.Type == RuleItemType.NonTerminal && rplDic.ContainsKey(item.Key))
                            item.Key = rplDic[item.Key];
                        else if (item.Type == RuleItemType.NonTerminal)
                            item.Key = $"{gr.Name}.{item.Key}";
                }
            }
        }

        private IEnumerable<Rule> _compileRules(IEnumerable<RuleSrc> srcRules, Dictionary<string, string> variables)
        {
            srcRules = _variablesProcessor.Process(srcRules, variables);
            srcRules = new CounterProcessor().Process(srcRules).ToArray();
            _checkUndefinedNonTerminals(srcRules);

            var list = new List<Rule>();
            var nullItems = srcRules.Where(x => !x.HasTerms)
                .Select(x => x.Name)
                .Distinct()
                .ToArray();
            
            Dictionary<string, List<Rule>> nameDic = new Dictionary<string, List<Rule>>();
            foreach (var srl in srcRules)
            {
                if (!srl.HasTerms)
                    continue;

                var validator = _valMan.GetValidator(srl);
                var terms = _compileTerms(srl, srl.Items, nullItems).ToArray();

                var rule = new Rule(srl.Name, 
                    terms, 
                    template: srl.Template, 
                    staticVars: srl.StaticVars.ToArray(),
                    isPossibleList: srl.IsPossibleList,
                    validator: validator,
                    isSystemIntermediate: srl.IsSystemIntermediate
                );
                list.Add(rule);
                
                if (!nameDic.ContainsKey(srl.Name))
                    nameDic[srl.Name] = new List<Rule>();
                
                nameDic[srl.Name].Add(rule);
            }
            
            var rezNameDic = nameDic.ToDictionary(kp=>kp.Key, kp=>kp.Value.ToArray());
            foreach (var rule in list)
            {
                foreach (var term in rule.Terms)
                {
                    if (!term.IsTerminal)
                    {
                        var nonTerm = (NonTerminal)term;
                        nonTerm.Rules = rezNameDic[nonTerm.Name];
                    }
                }
            }

            return list;
        }

        private void _checkUndefinedNonTerminals(IEnumerable<RuleSrc> srcRules)
        {
            var nonTeminalNames = srcRules.Select(x => x.Name).Distinct().ToArray();
            foreach (var rl in srcRules)
                foreach (var item in rl.Items)
                    if (item.Type==RuleItemType.NonTerminal && !nonTeminalNames.Contains(item.Key))
                        throw new CfgCompileException($"NonTerminal {item.Key} is undefined");
        }



        private IEnumerable<TermBase> _compileTerms(RuleSrc rule, IEnumerable<RuleItem> items, string[] nullItems)
        {
            foreach (var item in items)
            {
                var cond = _condMan.GetCondition(item);

                if (item.Type==RuleItemType.NonTerminal)
                    yield return new NonTerminal(item.Key,
                                                 isNullable: nullItems.Contains(item.Key),
                                                 localName: item.LocalName,
                                                 condition: cond,
                                                 isHead: item.IsHead  
                                                );
                else
                    yield return new Terminal(localName: item.LocalName,
                                              condition: cond,
                                              isHead: item.IsHead,
                                              semanticId: item.SemanticId
                                            );
            }
        }



    }
}