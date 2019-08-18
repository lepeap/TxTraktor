using NUnit.Framework;
using TxTraktor.Source.Model;
using TxTraktor.Source.Model.Extraction;

namespace TxtTractor.Test.Source
{
    [Parallelizable(ParallelScope.All)]
    public class Conditions
    {
        [Test]
        public void NonTerminalKeyPair()
        {
            Checker.CheckRule(
                "S -> T<cond1=value1>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", "value1") 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalKeyPairNegation()
        {
            Checker.CheckRule(
                "S -> T<~cond1=value1>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", "value1", true) 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalKeyPairCyrillic()
        {
            Checker.CheckRule(
                "S -> T<условие=значение_условия>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("условие", "значение_условия") 
                            })
                        })
                }
            );
        }
        
                
        [Test]
        public void NonTerminalKeyPairString()
        {
            Checker.CheckRule(
                "S -> T<условие=\"значение_условия\">",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("условие", "значение_условия") 
                            })
                        })
                }
            );
        }

        [Test]
        public void NonTerminalFlag()
        {
            Checker.CheckRule(
                "S -> T<cond1>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", false) 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalFlagNegation()
        {
            Checker.CheckRule(
                "S -> T<~cond1>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", true) 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalFlagCyrillic()
        {
            Checker.CheckRule(
                "S -> T<условие>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("условие", false) 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalList()
        {
            Checker.CheckRule(
                "S -> T<cond1=1,2,3>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", new []{"1","2","3"}) 
                            })
                        })
                }
            );
        }

        [Test]
        public void NonTerminalListString()
        {
            Checker.CheckRule(
                "S -> T<cond1=\"1\",\"2\",\"3\">",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", new []{"1","2","3"}) 
                            })
                        })
                }
            );
        }
        
                        
        [Test]
        public void NonTerminalListStringAndLiteral()
        {
            Checker.CheckRule(
                "S -> T<cond1=\"1\",2>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", new []{"1","2"}) 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalListNegation()
        {
            Checker.CheckRule(
                "S -> T<~cond1=1,2,3>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", new []{"1","2","3"}, true) 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalListCyrillic()
        {
            Checker.CheckRule(
                "S -> T<морф=сущ,прил,глаг>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("морф", new []{"сущ","прил","глаг"}) 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalTwoKeyPair()
        {
            Checker.CheckRule(
                "S -> T<cond1=value1;cond2=value2>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", "value1"),
                                new Condition("cond2", "value2") 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalKeyPairAndFlag()
        {
            Checker.CheckRule(
                "S -> T<cond1=value1;cond2>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", "value1"),
                                new Condition("cond2", false) 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalKeyPairAndList()
        {
            Checker.CheckRule(
                "S -> T<cond1=value1;cond2=val1,val2>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", "value1"),
                                new Condition("cond2", new []{"val1","val2"}) 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalAllTypes()
        {
            Checker.CheckRule(
                "S -> T<cond1=value1;cond2=val1,val2;cond3>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", conditions: new []
                            {
                                new Condition("cond1", "value1"),
                                new Condition("cond2", new []{"val1","val2"}),
                                new Condition("cond3", false) 
                            })
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalTwoKeyPairAndLocalName()
        {
            Checker.CheckRule(
                "S -> T<cond1=value1;cond2=value2> as name",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", 
                                localName: "name",
                                conditions: new []
                            {
                                new Condition("cond1", "value1"),
                                new Condition("cond2", "value2"),
                            })
                        },
                        new Template(new []
                        {
                            new TemplateItem<(string, string)>("Name", ("name", null), TemplateValueType.NameRef)
                        }))
                }
            );
        }
        
        [Test]
        public void NonTerminalThreeKeyPair()
        {
            Checker.CheckRule(
                "S -> T<cond1=value1;cond2=value2;cond3=value3>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", 
                                conditions: new []
                                {
                                    new Condition("cond1", "value1"),
                                    new Condition("cond2", "value2"),
                                    new Condition("cond3", "value3"),
                                })
                        })
                }
            );
        }
    }
}