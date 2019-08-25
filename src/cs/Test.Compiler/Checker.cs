using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TxTraktor.Compile;
using TxTraktor.Compile.Model;
using TxTraktor.Extension;
using TxTraktor.Tokenize;
using RuleSrc = TxTraktor.Source.Model.Rule;



namespace TxtTractor.Test.Compiler
{
    internal static class Checker
    {

        public static IGrammarCompiler Compiler => _createCompiler(null);

        private static IGrammarCompiler _createCompiler(IEnumerable<IExtension> extensions)
        {
            return new GrammarCompiler(new WordPunctTokenizer(), extensions);
        }
        
        internal static void CheckRules(IEnumerable<RuleSrc> srcRules, 
                                      IEnumerable<Rule> etalonRules,
                                      IEnumerable<IExtension> extensions=null,
                                      Dictionary<string, string> variables=null
                                      )
        {
            var resultRls = _createCompiler(extensions)
                                .CompileRules(srcRules, variables)
                                .ToArray();
            var etalonRls = etalonRules.ToArray();
            _checkRules(etalonRls, resultRls);

        }

        private static void _checkRules(Rule[] etalonRules, Rule[] resultRules)
        {
            Assert.AreEqual(etalonRules.Length,
                            resultRules.Length,
                            "Wrong rules count");
            
            for (int i = 0; i < etalonRules.Length; i++)
            {
                var etRule = etalonRules[i];
                var rzRule = resultRules[i];
                
                Assert.AreEqual(etRule.Name,
                                rzRule.Name,
                                "Wrong rule name");
                                
                Assert.AreEqual(etRule.IsStart,
                                rzRule.IsStart,
                                "Wrong start flag");
                
                
                Assert.AreEqual(etRule.IsPossibleList,
                                rzRule.IsPossibleList,
                                "Wrong list flag");
                
                Assert.AreEqual(etRule.HasTemplate,
                                rzRule.HasTemplate,
                                "Wrong template value");
                
                Assert.AreEqual(etRule.HasValidator,
                                rzRule.HasValidator,
                                "Wrong validator");
                
                Assert.AreEqual(etRule.IsSystemIntermediate,
                    rzRule.IsSystemIntermediate,
                    "Wrong system intermediate flag");
                
                if (etRule.HasValidator)
                    Assert.AreEqual(etRule.Validator.ToString(),
                                    rzRule.Validator.ToString(),
                                    "Wrong validator value");
                
                if (etRule.HasTemplate)
                    Source.Checker.CheckTemplate(etRule.Template, rzRule.Template);
                
                Assert.AreEqual(etRule.HasTerms,
                                rzRule.HasTerms,
                                "Wrong terms count");

                if (etRule.HasTerms)
                    _checkTerms(etRule.Terms.ToArray(), rzRule.Terms.ToArray());



            }
            
        }
        private static void _checkTerms(TermBase[] etTerms, TermBase[] rezTerms)
        {
            Assert.AreEqual(etTerms.Length,
                            rezTerms.Length,
                            "Wrong terms count");

            for (int i = 0; i < etTerms.Length; i++)
            {
                var etTerm = etTerms[i];
                var rezTerm = rezTerms[i];
                
                Assert.AreEqual(etTerm.LocalName,
                                rezTerm.LocalName,
                                "Wrong local name");
                
                Assert.AreEqual(etTerm.IsNullable,
                                rezTerm.IsNullable,
                                "Wrong nullable flag");
                
                Assert.AreEqual(etTerm.IsTerminal,
                                rezTerm.IsTerminal,
                                "Wrong term type");
                
                                
                Assert.AreEqual(etTerm.SemanticId,
                    rezTerm.SemanticId,
                    "Wrong semantic id");
                
                Assert.AreEqual(etTerm.ConditionKey,
                                rezTerm.ConditionKey,
                                "Wrong condition");
                
                Assert.AreEqual(etTerm.IsHead,
                                rezTerm.IsHead,
                                "Wrong head flag");

                if (!etTerm.IsTerminal)
                {
                    var etNonTerm = (NonTerminal)etTerm;
                    var rezNonTerm = (NonTerminal)rezTerm;
                    
                    Assert.AreEqual(etNonTerm.Name,
                                    rezNonTerm.Name,
                                    "Wrong nonterminal name");
                }
            }

        }       
    }
}