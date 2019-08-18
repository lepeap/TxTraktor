using NUnit.Framework;
using TxTraktor.Compile;
using TxTraktor.Compile.Condition;
using TxTraktor.Compile.Model;
using TxTraktor.Source.Model;
using RuleSrc = TxTraktor.Source.Model.Rule;
using Rule = TxTraktor.Compile.Model.Rule;

namespace TxtTractor.Test.Compiler
{
    [Parallelizable(ParallelScope.All)]
    public class Simple
    {
        [Test]
        public void OneTerminal()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []{new RuleItem(RuleItemType.Terminal, "123") })
                },
                new []
                {
                    new Rule("S", new []{new Terminal(condition: new TextCondition("123")) })
                }
            );
        }
        
        [Test]
        public void TwoTerminals()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.Terminal, "123"),
                        new RuleItem(RuleItemType.Terminal, "234")
                    })
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("234"))
                    })
                }
            );
        }
        
        [Test]
        public void OneRegex()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.Regex, "123")
                    })
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(condition: new RegexCondition("123"))
                    })
                }
            );
        }
        
        [Test]
        public void TwoRegex()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.Regex, "123"),
                        new RuleItem(RuleItemType.Regex, "234")
                    })
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(condition: new RegexCondition("123")),
                        new Terminal(condition: new RegexCondition("234"))
                    })
                }
            );
        }
        
                
        [Test]
        public void OneLemma()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.Lemma, "вечер")
                    })
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(condition: new LemmaCondition("вечер"))
                    })
                }
            );
        }
        
        [Test]
        public void TwoLemmas()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.Lemma, "час"),
                        new RuleItem(RuleItemType.Lemma, "вечер")
                    })
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(condition: new LemmaCondition("час")),
                        new Terminal(condition: new LemmaCondition("вечер"))
                    })
                }
            );
        }
        
        [Test]
        public void OneNonTerminal()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S1", new []
                    {
                        new RuleItem(RuleItemType.Terminal, "123")
                    }),
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.NonTerminal, "S1")
                    })
                },
                new []
                {
                    new Rule("S1", new []
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }),
                    new Rule("S", new []
                    {
                        new NonTerminal("S1") 
                    })
                }
            );
        }
        
        [Test]
        public void OneFullNonTerminal()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("Common.S1", new []
                    {
                        new RuleItem(RuleItemType.Terminal, "123")
                    }),
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.NonTerminal, "Common.S1")
                    })
                },
                new []
                {
                    new Rule("Common.S1", new []
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }),
                    new Rule("S", new []
                    {
                        new NonTerminal("Common.S1") 
                    })
                }
            );
        }
        
        [Test]
        public void TwoNonTerminal()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S1", new []
                    {
                        new RuleItem(RuleItemType.Terminal, "123")
                    }),
                    new RuleSrc("S2", new []
                    {
                        new RuleItem(RuleItemType.Terminal, "test")
                    }),
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.NonTerminal, "S1"),
                        new RuleItem(RuleItemType.NonTerminal, "S2")
                    })
                },
                new []
                {
                    new Rule("S1", new []
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }),
                    new Rule("S2", new []
                    {
                        new Terminal(condition: new TextCondition("test"))
                    }),
                    new Rule("S", new []
                    {
                        new NonTerminal("S1"),
                        new NonTerminal("S2")
                    })
                }
            );
        }

        
        [Test]
        public void OneTerminalWithLocalName()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S1", new []
                    {
                        new RuleItem(RuleItemType.Terminal, "123", localName:"t1")
                    })
                },
                new []
                {
                    new Rule("S1", new []
                    {
                        new Terminal(condition: new TextCondition("123"), localName:"t1")
                    })
                }
            );
        }
        
        [Test]
        public void TwoTerminalsWithLocalName()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S1", new []
                    {
                        new RuleItem(RuleItemType.Terminal, "123", localName:"t1"),
                        new RuleItem(RuleItemType.Terminal, "хер", localName:"t2")
                    })
                },
                new []
                {
                    new Rule("S1", new []
                    {
                        new Terminal(condition: new TextCondition("123"), localName:"t1"),
                        new Terminal(condition: new TextCondition("хер"), localName:"t2"),
                    })
                }
            );
        }
        
        [Test]
        public void ExceptionUndefinedNonTerminal()
        {
            var rls = new[]
            {
                new RuleSrc("S", new []
                {
                    new RuleItem(RuleItemType.NonTerminal, "S1")
                })
            };
            Assert.Throws<CfgCompileException>(() => Checker.Compiler.CompileRules(rls));
        }
    }
}