using NUnit.Framework;
using TxTraktor.Source.Model;
using TxTraktor.Source.Model.Extraction;

namespace TxtTractor.Test.Source
{
    [Parallelizable(ParallelScope.All)]
    public class Simple
    {
        [Test]
        public void OneRegex()
        {
            Checker.CheckRule(
                "S -> r\"123\"",
                new[]
                {
                   new Rule("S", 
                       new[]
                       {
                           new RuleItem(RuleItemType.Regex, "123")
                       })
                   }
                );
        }
        
        [Test]
        public void OneTerminal()
        {
            Checker.CheckRule(
                "S -> \"123\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "123")
                        })
                }
            );
        }
        
        [Test]
        public void OneTerminalWithLocalName()
        {
            Checker.CheckRule(
                "S -> \"123\" as numb1",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "123", localName: "numb1")
                        },
                        new Template(new []
                        {
                            new TemplateItem<(string, string)>("Numb1", ("numb1", null), TemplateValueType.NameRef)
                        }))
                }
            );
        }
        
        [Test]
        public void TwoTerminals()
        {
            Checker.CheckRule(
                "S -> \"123\" \"234\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "123"),
                            new RuleItem(RuleItemType.Terminal, "234")
                        })
                }
            );
        }
        
        [Test]
        public void OneMorphology()
        {
            Checker.CheckRule(
                "S -> m\"числ\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Morphology, "числ")
                        })
                }
            );
        }
        
        [Test]
        public void TwoRulesOneTerminalOneRegex()
        {
            Checker.CheckRule(
                "S -> \"123\" | r\"234\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "123")
                        }),
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Regex, "234")
                        })
                }
            );
        }       
        
        [Test]
        public void ThreeRulesTerminals()
        {
            Checker.CheckRule(
                "S -> \"123\" | \"234\" | \"345\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "123")
                        }),
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "234")
                        }),
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "345")
                        })
                }
            );
        }
        
        [Test]
        public void OneNonTerminal()
        {
            Checker.CheckRule(
                "S -> T",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T")
                        })
                }
            );
        }
        
        [Test]
        public void OneNonTerminalWithLocalName()
        {
            Checker.CheckRule(
                "S -> T as test",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", localName: "test")
                        },
                        new Template(new []
                        {
                            new TemplateItem<(string, string)>("Test", ("test", null), TemplateValueType.NameRef)
                        }))
                }
            );
        }
        
        [Test]
        public void TwoRulesOneNonTerminalWithLocalName()
        {
            Checker.CheckRule(
                "S -> T as test | F as test",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", localName: "test")
                        },
                        new Template(new []
                        {
                            new TemplateItem<(string, string)>("Test", ("test", null), TemplateValueType.NameRef)
                        })),
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "F", localName: "test")
                        },
                        new Template(new []
                        {
                            new TemplateItem<(string, string)>("Test", ("test", null), TemplateValueType.NameRef)
                        }))
                }
            );
        }
        
        [Test]
        public void NonTerminalAndTerminal()
        {
            Checker.CheckRule(
                "S -> T \"хер\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T"),
                            new RuleItem(RuleItemType.Terminal, "хер")
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalAndRegex()
        {
            Checker.CheckRule(
                "S -> T r\"хер\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T"),
                            new RuleItem(RuleItemType.Regex, "хер")
                        })
                }
            );
        }
        
        [Test]
        public void OneLemma()
        {
            Checker.CheckRule(
                "S -> l\"вечер\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Lemma, "вечер")
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalAndLemma()
        {
            Checker.CheckRule(
                "S -> T l\"хер\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T"),
                            new RuleItem(RuleItemType.Lemma, "хер")
                        })
                }
            );
        }
        
        [Test]
        public void FullNonTerminal()
        {
            Checker.CheckRule(
                "S -> Common.Test",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "Common.Test")
                        })
                }
            );
        }
        
        [Test]
        public void Variable()
        {
            Checker.CheckRule(
                "S -> $test",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.VariableName, "test")
                        })
                }
            );
        }
        
        [Test]
        public void Quote()
        {
            Checker.CheckRule(
                "S -> &q",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "\"")
                        })
                }
            );
        }
        
        [Test]
        public void TwoLemmasOnOneLine()
        {
            Checker.CheckRule(
                "S -> l\"военный\" l\"округ\" ",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Lemma, "военный"),
                            new RuleItem(RuleItemType.Lemma, "округ")
                        })
                }
            );
        }
                
        [Test]
        public void TwoRegexOnOneLine()
        {
            Checker.CheckRule(
                "S -> r\"военный\" r\"округ\" ",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Regex, "военный"),
                            new RuleItem(RuleItemType.Regex, "округ")
                        })
                }
            );
        }
        
        [Test]
        public void TwoMorphsOnOneLine()
        {
            Checker.CheckRule(
                "S -> m\"военный\" m\"округ\" ",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Morphology, "военный"),
                            new RuleItem(RuleItemType.Morphology, "округ")
                        })
                }
            );
        }

                
        [Test]
        public void Everything()
        {
            Checker.CheckRule(
                "S -> &. ",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Everything, string.Empty)
                        })
                }
            );
        }
        
        

    }
}