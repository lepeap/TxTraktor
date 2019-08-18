using NUnit.Framework;
using TxTraktor.Source.Model;
using TxTraktor.Source.Model.Extraction;

namespace TxtTractor.Test.Source
{
    [Parallelizable(ParallelScope.All)]
    public class InlineRules
    {
        [Test]
        public void TerminalAndInlineRule()
        {
            Checker.CheckRule(
                "S -> \"хер\" (\"123\")",
                new[]
                {
                    new Rule("S:1", 
                        new[]
                        {
                            
                            new RuleItem(RuleItemType.Terminal, "123")
                        }),
                    new Rule("S", 
                        new[]
                        {
                            
                            new RuleItem(RuleItemType.Terminal, "хер"),
                            new RuleItem(RuleItemType.NonTerminal, "S:1"),
                        })
                    
                }
            );
        }
        
        [Test]
        public void TerminalAndInlineRuleWithCounter()
        {
            Checker.CheckRule(
                "S -> \"хер\" (\"123\")+",
                new[]
                {
                    new Rule("S:1", 
                        new[]
                        {
                            
                            new RuleItem(RuleItemType.Terminal, "123")
                        }),
                    new Rule("S", 
                        new[]
                        {
                            
                            new RuleItem(RuleItemType.Terminal, "хер"),
                            new RuleItem(RuleItemType.NonTerminal, "S:1", Counter.Plus),
                        })
                    
                }
            );
        }
        
        [Test]
        public void TerminalAndInlineRuleWithHead()
        {
            Checker.CheckRule(
                "S -> \"хер\" !(\"123\")",
                new[]
                {
                    new Rule("S:1", 
                        new[]
                        {
                            
                            new RuleItem(RuleItemType.Terminal, "123")
                        }),
                    new Rule("S", 
                        new[]
                        {
                            
                            new RuleItem(RuleItemType.Terminal, "хер"),
                            new RuleItem(RuleItemType.NonTerminal, "S:1", isHead: true),
                        })
                    
                }
            );
        }
        
                
        [Test]
        public void TerminalAndInlineRuleWithCondition()
        {
            Checker.CheckRule(
                "S -> \"хер\" (\"123\")<условие>",
                new[]
                {
                    new Rule("S:1", 
                        new[]
                        {
                            
                            new RuleItem(RuleItemType.Terminal, "123")
                        }),
                    new Rule("S", 
                        new[]
                        {
                            
                            new RuleItem(RuleItemType.Terminal, "хер"),
                            new RuleItem(RuleItemType.NonTerminal, "S:1", conditions: new []
                            {
                                new Condition("условие", false) 
                            }),
                        })
                    
                }
            );
        }
        
        [Test]
        public void TerminalAndInlineRuleWithLocalName()
        {
            Checker.CheckRule(
                "S -> \"хер\" (\"123\") as test",
                new[]
                {
                    new Rule("S:1", 
                        new[]
                        {
                            
                            new RuleItem(RuleItemType.Terminal, "123")
                        }),
                    new Rule("S", 
                        new[]
                        {
                            
                            new RuleItem(RuleItemType.Terminal, "хер"),
                            new RuleItem(RuleItemType.NonTerminal, "S:1", localName: "test")
                        },
                        new Template(new []
                        {
                            new TemplateItem<(string, string)>("Test", ("test", null), TemplateValueType.NameRef)
                        }))
                    
                }
            );
        }
    }
}