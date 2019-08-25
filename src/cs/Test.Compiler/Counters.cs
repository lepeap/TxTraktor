using NUnit.Framework;
using TxTraktor.Compile.Condition;
using TxTraktor.Compile.Model;
using TxTraktor.Source.Model;
using TxTraktor.Source.Model.Extraction;
using RuleSrc = TxTraktor.Source.Model.Rule;
using Rule = TxTraktor.Compile.Model.Rule;

namespace TxtTractor.Test.Compiler
{
    [Parallelizable(ParallelScope.All)]
    public class Counters
    {
        [Test]
        public void TerminalStar()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Star)})
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true)
                    }),
                    new Rule("TERMINAL_0_STAR", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true)
                    }, isPossibleList: true)
                }
            );
        }

        [Test]
        public void TerminalStarWithLocalName()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[]
                    {
                        new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Star, localName: "test")
                    })
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true, localName: "test")
                    }),
                    new Rule("TERMINAL_0_STAR", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true)
                    }, isPossibleList: true)
                }
            );
        }


        [Test]
        public void TerminalCacheStar()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Star)}),
                    new RuleSrc("S1", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Star)})
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true)
                    }),
                    new Rule("TERMINAL_0_STAR", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("S1", new[]
                    {
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true)
                    }),
                }
            );
        }

        [Test]
        public void TerminalCacheStarWithLocalName()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[]
                    {
                        new RuleItem(RuleItemType.Terminal,
                            "123",
                            counter: Counter.Star,
                            localName: "test")
                    }),
                    new RuleSrc("S1", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Star)})
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true, localName: "test")
                    }),
                    new Rule("TERMINAL_0_STAR", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("S1", new[]
                    {
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true)
                    }),
                }
            );
        }

        [Test]
        public void NonTerminalStar()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123")}),
                    new RuleSrc("S1", new[] {new RuleItem(RuleItemType.NonTerminal, "S", counter: Counter.Star)})
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }),
                    new Rule("S1", new[]
                    {
                        new NonTerminal("S_STAR", isNullable: true)
                    }),
                    new Rule("S_STAR", new TermBase[]
                    {
                        new NonTerminal("S"),
                        new NonTerminal("S_STAR", isNullable: true)
                    }, isPossibleList: true)
                }
            );
        }

        [Test]
        public void NonTerminalCacheStar()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123")}),
                    new RuleSrc("S1", new[]
                    {
                        new RuleItem(RuleItemType.NonTerminal, "S", counter: Counter.Star)
                    }),
                    new RuleSrc("S2", new[]
                    {
                        new RuleItem(RuleItemType.NonTerminal, "S", counter: Counter.Star)
                    })
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }),
                    new Rule("S1", new[]
                    {
                        new NonTerminal("S_STAR", isNullable: true)
                    }),
                    new Rule("S_STAR", new TermBase[]
                    {
                        new NonTerminal("S"),
                        new NonTerminal("S_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("S2", new[]
                    {
                        new NonTerminal("S_STAR", isNullable: true)
                    })
                }
            );
        }

        [Test]
        public void TerminalQuestion()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Question)})
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_QUESTION", isNullable: true)
                    }),
                    new Rule("TERMINAL_0_QUESTION",
                        new TermBase[] {new Terminal(condition: new TextCondition("123"))},
                        template: new Template(new[] {new TemplateItem<(int, string)>("Value", (0, null), TemplateValueType.NumberRef)}),
                        isSystemIntermediate: true)
                }
            );
        }

        [Test]
        public void TerminalQuestionWithLocalName()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[]
                    {
                        new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Question, localName: "test")
                    })
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_QUESTION", isNullable: true, localName: "test")
                    }),
                    new Rule("TERMINAL_0_QUESTION", 
                        new TermBase[]{new Terminal(condition: new TextCondition("123"))},
                        template: new Template(new[] {new TemplateItem<(int, string)>("Value", (0, null), TemplateValueType.NumberRef)}),
                        isSystemIntermediate: true
                        )
                }
            );
        }

        [Test]
        public void TerminalCacheQuestion()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Question)}),
                    new RuleSrc("S1", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Question)})
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_QUESTION", isNullable: true)
                    }),
                    new Rule("TERMINAL_0_QUESTION", 
                        new TermBase[] {new Terminal(condition: new TextCondition("123"))}, 
                        template: new Template(new[] {new TemplateItem<(int, string)>("Value", (0, null), TemplateValueType.NumberRef)}),
                        isSystemIntermediate: true),
                    new Rule("S1", new[]
                    {
                        new NonTerminal("TERMINAL_0_QUESTION", isNullable: true)
                    }),
                }
            );
        }

        [Test]
        public void TerminalCacheQuestionWithLocalName()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[]
                    {
                        new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Question, localName: "test")
                    }),
                    new RuleSrc("S1", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Question)})
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_QUESTION", isNullable: true, localName: "test")
                    }),
                    new Rule("TERMINAL_0_QUESTION", 
                        new TermBase[]{new Terminal(condition: new TextCondition("123"))},
                        template: new Template(new[] {new TemplateItem<(int, string)>("Value", (0, null), TemplateValueType.NumberRef)}),
                        isSystemIntermediate: true
                        ),
                    new Rule("S1", new[]
                    {
                        new NonTerminal("TERMINAL_0_QUESTION", isNullable: true)
                    }),
                }
            );
        }

        [Test]
        public void NonTerminalQuestion()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123")}),
                    new RuleSrc("S1", new[] {new RuleItem(RuleItemType.NonTerminal, "S", counter: Counter.Question)})
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }),
                    new Rule("S1", new[]
                    {
                        new NonTerminal("S_QUESTION", isNullable: true)
                    }),
                    new Rule("S_QUESTION", new TermBase[]
                        {
                            new NonTerminal("S")
                        },
                        template: new Template(new[]
                        {
                            new TemplateItem<(int, string)>("Value", (0, null), TemplateValueType.NumberRef)
                        }),
                        isSystemIntermediate: true)
                }
            );
        }

        [Test]
        public void NonTerminalCacheQuestion()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123")}),
                    new RuleSrc("S1", new[]
                    {
                        new RuleItem(RuleItemType.NonTerminal, "S", counter: Counter.Question)
                    }),
                    new RuleSrc("S2", new[]
                    {
                        new RuleItem(RuleItemType.NonTerminal, "S", counter: Counter.Question)
                    })
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }),
                    new Rule("S1", new[]
                    {
                        new NonTerminal("S_QUESTION", isNullable: true)
                    }),
                    new Rule("S_QUESTION", new TermBase[]
                        {
                            new NonTerminal("S")
                        }, template: new Template(new[]
                        {
                            new TemplateItem<(int, string)>("Value", (0, null), TemplateValueType.NumberRef)
                        }),
                        isSystemIntermediate: true),
                    new Rule("S2", new[]
                    {
                        new NonTerminal("S_QUESTION", isNullable: true)
                    })
                }
            );
        }

        [Test]
        public void TerminalPlus()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Plus)})
                },
                new[]
                {
                    new Rule("S", new TermBase[]
                    {
                        new NonTerminal("TERMINAL_0_PLUS")
                    }),
                    new Rule("TERMINAL_0_PLUS", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_1_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("TERMINAL_1_STAR", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_1_STAR", isNullable: true)
                    }, isPossibleList: true)
                }
            );
        }

        [Test]
        public void TerminalPlusWithLocalName()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[]
                    {
                        new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Plus, localName: "test")
                    })
                },
                new[]
                {
                    new Rule("S", new TermBase[]
                    {
                        new NonTerminal("TERMINAL_0_PLUS", localName: "test")
                    }),
                    new Rule("TERMINAL_0_PLUS", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_1_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("TERMINAL_1_STAR", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_1_STAR", isNullable: true)
                    }, isPossibleList: true)
                }
            );
        }

        [Test]
        public void TerminalCachePlus()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Plus)}),
                    new RuleSrc("S1", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Plus)})
                },
                new[]
                {
                    new Rule("S", new TermBase[]
                    {
                        new NonTerminal("TERMINAL_0_PLUS")
                    }),
                    new Rule("TERMINAL_0_PLUS", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_1_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("TERMINAL_1_STAR", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_1_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("S1", new TermBase[]
                    {
                        new NonTerminal("TERMINAL_0_PLUS")
                    })
                }
            );
        }

        [Test]
        public void TerminalCachePlusWithLocalName()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S",
                        new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Plus, localName: "test")}),
                    new RuleSrc("S1", new[] {new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Plus)})
                },
                new[]
                {
                    new Rule("S", new TermBase[]
                    {
                        new NonTerminal("TERMINAL_0_PLUS", localName: "test")
                    }),
                    new Rule("TERMINAL_0_PLUS", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_1_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("TERMINAL_1_STAR", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_1_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("S1", new TermBase[]
                    {
                        new NonTerminal("TERMINAL_0_PLUS")
                    })
                }
            );
        }

        [Test]
        public void NonTerminalPlus()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123")}),
                    new RuleSrc("S1", new[] {new RuleItem(RuleItemType.NonTerminal, "S", counter: Counter.Plus)})
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }),
                    new Rule("S1", new[]
                    {
                        new NonTerminal("S_PLUS")
                    }),
                    new Rule("S_PLUS", new[]
                    {
                        new NonTerminal("S"),
                        new NonTerminal("S_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("S_STAR", new TermBase[]
                    {
                        new NonTerminal("S"),
                        new NonTerminal("S_STAR", isNullable: true)
                    }, isPossibleList: true)
                }
            );
        }

        [Test]
        public void NonTerminalCachePlus()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[] {new RuleItem(RuleItemType.Terminal, "123")}),
                    new RuleSrc("S1", new[]
                    {
                        new RuleItem(RuleItemType.NonTerminal, "S", counter: Counter.Plus)
                    }),
                    new RuleSrc("S2", new[]
                    {
                        new RuleItem(RuleItemType.NonTerminal, "S", counter: Counter.Plus)
                    })
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }),
                    new Rule("S1", new[]
                    {
                        new NonTerminal("S_PLUS")
                    }),
                    new Rule("S_PLUS", new TermBase[]
                    {
                        new NonTerminal("S"),
                        new NonTerminal("S_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("S_STAR", new TermBase[]
                    {
                        new NonTerminal("S"),
                        new NonTerminal("S_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("S2", new[]
                    {
                        new NonTerminal("S_PLUS")
                    })
                }
            );
        }

        [Test]
        public void TerminalNumbersDefaultMin()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S",
                        new[] {new RuleItem(RuleItemType.Terminal, "123", Counter.Number, new CounterValue(0, 3))}),
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_COUNTER_0_3", isNullable: true)
                    }),
                    new Rule("TERMINAL_0_COUNTER_0_3", new[]
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }, isPossibleList: true),
                    new Rule("TERMINAL_0_COUNTER_0_3", new[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("123"))
                    }, isPossibleList: true),
                    new Rule("TERMINAL_0_COUNTER_0_3", new[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("123"))
                    }, isPossibleList: true),
                }
            );
        }

        [Test]
        public void TerminalNumbers()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S",
                        new[] {new RuleItem(RuleItemType.Terminal, "123", Counter.Number, new CounterValue(1, 3))}),
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_COUNTER_1_3")
                    }),
                    new Rule("TERMINAL_0_COUNTER_1_3", new[]
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }, isPossibleList: true),
                    new Rule("TERMINAL_0_COUNTER_1_3", new[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("123"))
                    }, isPossibleList: true),
                    new Rule("TERMINAL_0_COUNTER_1_3", new[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("123"))
                    }, isPossibleList: true),
                }
            );
        }

        [Test]
        public void TerminalNumbersWithLocalName()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[]
                    {
                        new RuleItem(
                            RuleItemType.Terminal,
                            "123",
                            Counter.Number,
                            new CounterValue(1, 3),
                            localName: "test"
                        )
                    }),
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_COUNTER_1_3", localName: "test")
                    }),
                    new Rule("TERMINAL_0_COUNTER_1_3", new[]
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }, isPossibleList: true),
                    new Rule("TERMINAL_0_COUNTER_1_3", new[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("123"))
                    }, isPossibleList: true),
                    new Rule("TERMINAL_0_COUNTER_1_3", new[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("123"))
                    }, isPossibleList: true),
                }
            );
        }

        [Test]
        public void NonTerminalNumbers()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("T", new[] {new RuleItem(RuleItemType.Terminal, "123")}),
                    new RuleSrc("S",
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", Counter.Number, new CounterValue(1, 3))
                        }),
                },
                new[]
                {
                    new Rule("T", new[]
                    {
                        new Terminal(condition: new TextCondition("123"))
                    }),
                    new Rule("S", new[]
                    {
                        new NonTerminal("T_COUNTER_1_3")
                    }),
                    new Rule("T_COUNTER_1_3", new[]
                    {
                        new NonTerminal("T")
                    }, isPossibleList: true),
                    new Rule("T_COUNTER_1_3", new[]
                    {
                        new NonTerminal("T"),
                        new NonTerminal("T")
                    }, isPossibleList: true),
                    new Rule("T_COUNTER_1_3", new[]
                    {
                        new NonTerminal("T"),
                        new NonTerminal("T"),
                        new NonTerminal("T")
                    }, isPossibleList: true)
                }
            );
        }

        [Test]
        public void TwoTerminalsStarAndQuestion()
        {
            Checker.CheckRules(
                new[]
                {
                    new RuleSrc("S", new[]
                    {
                        new RuleItem(RuleItemType.Terminal, "123", counter: Counter.Star),
                        new RuleItem(RuleItemType.Terminal, "234", counter: Counter.Question)
                    })
                },
                new[]
                {
                    new Rule("S", new[]
                    {
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true),
                        new NonTerminal("TERMINAL_1_QUESTION", isNullable: true)
                    }),
                    new Rule("TERMINAL_0_STAR", new TermBase[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new NonTerminal("TERMINAL_0_STAR", isNullable: true)
                    }, isPossibleList: true),
                    new Rule("TERMINAL_1_QUESTION", 
                        new TermBase[]{new Terminal(condition: new TextCondition("234"))},
                        template: new Template(new[] {new TemplateItem<(int, string)>("Value", (0, null), TemplateValueType.NumberRef)}),
                        isSystemIntermediate: true)
                }
            );
        }
    }
}