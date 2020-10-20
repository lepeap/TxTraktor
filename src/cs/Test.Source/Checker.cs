using System;
using System.Linq;
using NUnit.Framework;
using TxTraktor;
using TxTraktor.Source;
using TxTraktor.Source.Model;
using TxTraktor.Source.Model.Extraction;


namespace TxtTractor.Test.Source
{
    [Parallelizable(ParallelScope.Self)]
    internal static class Checker
    {
        static IGrammarParser _parser = new GrammarParser(null);

        internal static void CheckGrammar(string text, Grammar etalonGrammar)
        {
            var rezGrammar = _parser.Parse("Test", text);
            
            Assert.AreEqual(etalonGrammar.Language, 
                rezGrammar.Language,
                "Wrong grammar lang"
            );
            
            Assert.AreEqual(etalonGrammar.Name, 
                            rezGrammar.Name,
                            "Wrong grammar name"
                            );
            
            Assert.AreEqual(etalonGrammar.Key, 
                rezGrammar.Key,
                "Wrong grammar key"
            );
            
            _checkRules(rezGrammar.Rules.ToArray(), etalonGrammar.Rules.ToArray());
            
            Assert.AreEqual(etalonGrammar.HasGrammarImports, 
                rezGrammar.HasGrammarImports,
                "Wrong grammars imports count"
            );
            if (etalonGrammar.HasGrammarImports)
            {
                _checkGrammarImports(etalonGrammar.Imports.ToArray(), rezGrammar.Imports.ToArray());
            }
            
        }

        private static void _checkGrammarImports(Import[] etalonGrammarImports, Import[] resultGrammarImports)
        {
            Assert.AreEqual(etalonGrammarImports.Length,
                resultGrammarImports.Length,
                "Wrong grammars imports count"
            );

            for (int i = 0; i < etalonGrammarImports.Length; i++)
            {
                var etImport = etalonGrammarImports[i];
                var rezImport = resultGrammarImports[i];

                Assert.AreEqual(etImport.Name,
                    rezImport.Name,
                    "Wrong grammar import name"
                );

                Assert.AreEqual(etImport.LocalName,
                    rezImport.LocalName,
                    "Wrong grammar import local name"
                );

                Assert.AreEqual(etImport.Source,
                    rezImport.Source,
                    "Wrong grammar import source grammar"
                );
                
                Assert.AreEqual(etImport.Type,
                    rezImport.Type,
                    "Wrong grammar import type"
                );
            }
        }

        
        internal static void CheckRule(string text, Rule[] etalonRules)
        {
            var grammarText = $"grammar Test;\n{text};";
            var gram = _parser.Parse("Test", grammarText);
            Assert.IsNotNull(gram, "Syntax error in rule");
            var resultRules = gram.Rules.ToArray();
            _checkRules(resultRules, etalonRules);
        }

        private static void _checkRules(Rule[] resultRules, Rule[] etalonRules)
        {
            Assert.AreEqual(etalonRules.Length,
                resultRules.Length,
                "Wrong rules count");


            for (int i = 0; i < etalonRules.Length; i++)
            {
                var etRule = etalonRules[i];
                var rezRule = resultRules[i];
                
                Assert.AreEqual(etRule.HasTemplate,
                    rezRule.HasTemplate,
                    "Wrong template"
                );
                
                if (etRule.HasTemplate)
                    CheckTemplate(etRule.Template, rezRule.Template);
                
                // сравниваем имена, если к имени был добавлен гуид, то откидываем его и сравниваем основу
                if (!rezRule.WasInline)
                    Assert.AreEqual(etRule.Name,
                        rezRule.Name,
                        "Wrong rule name");
                else
                    Assert.AreEqual(etRule.Name.Split(":")[0],
                        rezRule.Name.Split(":")[0],
                        "Wrong rule name");

                Assert.AreEqual(etRule.Items.Count(),
                    rezRule.Items.Count(),
                    "Wrong rule items count");

                var etItems = etRule.Items.ToArray();
                var rezItems = rezRule.Items.ToArray();
                _checkRuleItems(etItems, rezItems);
                
                Assert.AreEqual(etRule.HasStaticVars,
                    rezRule.HasStaticVars,
                    "Wrong static vars"
                );

                if (etRule.HasStaticVars)
                {
                    var etVars = etRule.StaticVars.ToArray();
                    var rezVars = rezRule.StaticVars.ToArray();
                    Assert.AreEqual(etVars.Length,
                                    rezVars.Length,
                                    "Wrong static vars count");

                    for (int j = 0; j < etRule.StaticVars.Count(); j++)
                    {
                        _checkTemplateItem(etVars[j], rezVars[j]);
                    }
                }
                
                
                Assert.AreEqual(etRule.ExtensionType,
                    rezRule.ExtensionType,
                    "Wrong extension type"
                );
                
                Assert.AreEqual(etRule.ExtensionQuery,
                    rezRule.ExtensionQuery,
                    "Wrong extension query"
                );
            }
        }
        
        private static void _checkRuleItems(RuleItem[] etItems, RuleItem[] rezItems)
        {
            for (int j = 0; j < etItems.Length; j++)
            {
                var etItem = etItems[j];
                var rezItem = rezItems[j];
                
                if (etItem.Type == RuleItemType.NonTerminal && etItem.Key.Contains(":"))
                    Assert.AreEqual(etItem.Key.Split(":")[0],
                        rezItem.Key.Split(":")[0],
                        "Wrong rule name");
                else
                    Assert.AreEqual(etItem.Key,
                        rezItem.Key,
                        "Wrong RuleItem key");
                
                Assert.AreEqual(etItem.IsHead,
                    rezItem.IsHead,
                    "Wrong IsHead flag");

                Assert.AreEqual(etItem.Type,
                    rezItem.Type,
                    "Wrong RuleItem type");
                
                Assert.AreEqual(etItem.Counter,
                    rezItem.Counter,
                    "Wrong RuleItem counter");
                
                Assert.AreEqual(etItem.CounterValue==null,
                    rezItem.CounterValue==null,
                    "Wrong RuleItem counterValue");

                if (etItem.CounterValue != null && rezItem.CounterValue != null)
                {
                    Assert.AreEqual(etItem.CounterValue.MinValue,
                        rezItem.CounterValue.MinValue,
                        "Wrong RuleItem counterValue (min value)");
                    
                    Assert.AreEqual(etItem.CounterValue.MaxValue,
                        rezItem.CounterValue.MaxValue,
                        "Wrong RuleItem counterValue (max value)");
                }

                Assert.AreEqual(etItem.LocalName,
                    rezItem.LocalName,
                    "Wrong RuleItem localName");

                Assert.AreEqual(etItem.HasConditions,
                    rezItem.HasConditions,
                    "Wrong RuleItem conditions count");

                if (etItem.HasConditions)
                {
                    Assert.AreEqual(etItem.Conditions.Count(),
                        rezItem.Conditions.Count(),
                        "Wrong RuleItem conditions count");

                    var etConds = etItem.Conditions.ToArray();
                    var rezConds = rezItem.Conditions.ToArray();
                    for (int k = 0; k < etConds.Length; k++)
                    {
                        var etCond = etConds[k];
                        var rezCond = rezConds[k];

                        Assert.AreEqual(etCond.Key,
                            rezCond.Key,
                            "Wrong RuleItem condition key");

                        Assert.AreEqual(etCond.Negation,
                            rezCond.Negation,
                            "Wrong RuleItem condition negation flag");

                        Assert.AreEqual(etCond.Values,
                            rezCond.Values,
                            "Wrong RuleItem condition values");
                    }
                }
            }
        }

        public static void CheckTemplate(Template etTemplate, Template rezTemplate)
        {
            Assert.AreEqual(etTemplate.Name, 
                rezTemplate.Name,
                "Wrong template name");
            
            for (int j = 0; j < etTemplate.ItemsCount; j++)
            {
                var etItem = etTemplate[j];
                var rezItem = rezTemplate[j];

                _checkTemplateItem(etItem, rezItem);
            }
        }

        private static void _checkTemplateItem(TemplateItemBase etItem, TemplateItemBase rezItem)
        {
            Assert.AreEqual(etItem.Name,
                rezItem.Name,
                "Wrong template item name");
                    
            Assert.AreEqual(etItem.Type,
                rezItem.Type,
                "Wrong template item value type");

            Assert.AreEqual(etItem.GetType(),
                rezItem.GetType(),
                "Wrong value type");

            switch (etItem.Type)
            {
                case TemplateValueType.List:
                    var etValueItem = etItem.As<TemplateItem<TemplateItemBase[]>>();
                    var rezValueItem = rezItem.As<TemplateItem<TemplateItemBase[]>>();
                    Assert.AreEqual(etValueItem.Value.Length,
                        rezValueItem.Value.Length,
                        "Wrong template item list length");

                    for (int i = 0; i < etValueItem.Value.Length; i++)
                    {
                        _checkTemplateItem(etValueItem.Value[i], rezValueItem.Value[i]);
                    }
                    break;
                case TemplateValueType.NameRef:
                    _checkPropertyValue<(string, string)>(etItem, rezItem);
                    break;
                case TemplateValueType.String:
                    _checkPropertyValue<string>(etItem, rezItem);
                    break;
                case TemplateValueType.NumberRef:
                    _checkPropertyValue<(int, string)>(etItem, rezItem);
                    break;
                case TemplateValueType.Integer:
                    _checkPropertyValue<int>(etItem, rezItem);
                    break;
                case TemplateValueType.Float:
                    Assert.AreEqual(
                        etItem.As<TemplateItem<float>>().Value,
                        rezItem.As<TemplateItem<float>>().Value,
                        0.0001,
                        "Wrong property value");
                    break;
                case TemplateValueType.Bool:
                    _checkPropertyValue<bool>(etItem, rezItem);
                    break;
                    
                default:
                    throw new NotImplementedException();
                        
            }
        }

        private static void _checkPropertyValue<T>(TemplateItemBase etProp, TemplateItemBase rezProp)
        {
            
            Assert.AreEqual(etProp.As<TemplateItem<T>>().Value,
                            rezProp.As<TemplateItem<T>>().Value,
                            "Wrong property value");
        }
        
        public static void CheckTemplate(string template, Template etalonTemplate)
        {
            CheckRule(
                $"S[{template}] -> T",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T")
                        },
                        etalonTemplate
                    )
                }
            );
        }
        

    }
}