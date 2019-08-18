using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TxTraktor.Compile;
using TxTraktor.Compile.Condition;
using TxTraktor.Compile.Model;
using TxTraktor.Extension;
using TxTraktor.Source.Model;
using RuleSrc = TxTraktor.Source.Model.Rule;
using Rule = TxTraktor.Compile.Model.Rule;

namespace TxtTractor.Test.Compiler
{
    [Parallelizable(ParallelScope.All)]
    public class Extensions
    {
        private IExtension _createExtensions(string extName, IEnumerable<Dictionary<string, string>> results)
        {
            var ext = Substitute.For<IExtension>();
            ext.Name.Returns(extName);
            ext.Process(Arg.Any<string>()).Returns(results);
            return ext;
        }
        
        [Test]
        public void OneItem()
        {
            var exts = new[]
            {
                _createExtensions("test", new[]
                {
                    new Dictionary<string, string>()
                    {
                        {"test", "123"}
                    }
                })
            };
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", 
                        new []
                                {
                                    new RuleItem(RuleItemType.VariableName, "test")
                                },
                        extensionType: "test",
                        extensionQuery: "test")
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(localName: "test", condition: new TextCondition("123"))
                    })
                },
                extensions: exts
            );
        }
        
        [Test]
        public void ThreeItems()
        {
            var exts = new[]
            {
                _createExtensions("test", new[]
                {
                    new Dictionary<string, string>()
                    {
                        {"test", "123"}
                    },
                    new Dictionary<string, string>()
                    {
                        {"test", "234"}
                    },
                    new Dictionary<string, string>()
                    {
                        {"test", "345"}
                    }
                })
            };
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", 
                        new []
                        {
                            new RuleItem(RuleItemType.VariableName, "test")
                        },
                        extensionType: "test",
                        extensionQuery: "test")
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(localName: "test", condition: new TextCondition("123"))
                    }),
                    new Rule("S", new []
                    {
                        new Terminal(localName: "test", condition: new TextCondition("234"))
                    }),
                    new Rule("S", new []
                    {
                        new Terminal(localName: "test",condition: new TextCondition("345"))
                    })
                },
                extensions: exts
            );
        }
        
        [Test]
        public void SemanticId()
        {
            var exts = new[]
            {
                _createExtensions("test", new[]
                {
                    new Dictionary<string, string>()
                    {
                        {"test", "123"},
                        {"test_id", "id_123"}
                    }
                })
            };
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", 
                        new []
                        {
                            new RuleItem(RuleItemType.VariableName, "test")
                        },
                        extensionType: "test",
                        extensionQuery: "test")
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(localName: "test",condition: new TextCondition("123"), semanticId: "id_123")
                    })
                },
                extensions: exts
            );
        }
        
        [Test]
        public void OneLemma()
        {
            var exts = new[]
            {
                _createExtensions("test", new[]
                {
                    new Dictionary<string, string>()
                    {
                        {"test", "123"}
                    }
                })
            };
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", 
                        new []
                        {
                            new RuleItem(
                                RuleItemType.VariableName, 
                                "test",
                                conditions: new []
                                {
                                    new Condition("lemma", false)
                                })
                        },
                        extensionType: "test",
                        extensionQuery: "test")
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(localName: "test",condition: new LemmaCondition("123"))
                    })
                },
                extensions: exts
            );
        }
        
        [Test]
        public void OneRegex()
        {
            var exts = new[]
            {
                _createExtensions("test", new[]
                {
                    new Dictionary<string, string>()
                    {
                        {"test", "123"}
                    }
                })
            };
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", 
                        new []
                        {
                            new RuleItem(
                                RuleItemType.VariableName, 
                                "test",
                                conditions: new []
                                {
                                    
                                    new Condition("regex", false)
                                })
                        },
                        extensionType: "test",
                        extensionQuery: "test")
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(localName: "test",condition: new RegexCondition("123"))
                    })
                },
                extensions: exts
            );
        }

        [Test]
        public void MultipleTypeConditionsException()
        {
            Assert.Throws<CfgCompileException>(() =>
            {
                var exts = new[]
                {
                    _createExtensions("test", new[]
                    {
                        new Dictionary<string, string>()
                        {
                            {"test", "123"}
                        }
                    })
                };
                Checker.CheckRules(
                    new []
                    {
                        new RuleSrc("S", 
                            new []
                            {
                                new RuleItem(
                                    RuleItemType.VariableName, 
                                    "test",
                                    conditions: new []
                                    {
                                        new Condition("lemma", false),
                                        new Condition("regex", false)
                                    })
                            },
                            extensionType: "test",
                            extensionQuery: "test")
                    },
                    new Rule[] {},
                    extensions: exts
                );
            });
        }
        
        [Test]
        public void NoKeyInItemException()
        {
            Assert.Throws<CfgCompileException>(() =>
            {
                var exts = new[]
                {
                    _createExtensions("test", new[]
                    {
                        new Dictionary<string, string>()
                        {
                            {"test", "123"}
                        }
                    })
                };
                Checker.CheckRules(
                    new []
                    {
                        new RuleSrc("S", 
                            new []
                            {
                                new RuleItem(
                                    RuleItemType.VariableName, 
                                    "test1")
                            },
                            extensionType: "test",
                            extensionQuery: "test")
                    },
                    new Rule[] {},
                    extensions: exts
                );
            });
        }
        
        [Test]
        public void UndefinedExtensionException()
        {
            Assert.Throws<CfgCompileException>(() =>
            {
                var exts = new[]
                {
                    _createExtensions("test", new[]
                    {
                        new Dictionary<string, string>()
                        {
                            {"test", "123"}
                        }
                    })
                };
                Checker.CheckRules(
                    new []
                    {
                        new RuleSrc("S", 
                            new []
                            {
                                new RuleItem(
                                    RuleItemType.VariableName, 
                                    "test1")
                            },
                            extensionType: "test1",
                            extensionQuery: "test")
                    },
                    new Rule[] {},
                    extensions: exts
                );
            });
        }
        
        [Test]
        public void NoExtensionsButExtensionsInGrammar()
        {
            Assert.Throws<CfgCompileException>(() =>
            {
                var exts = new IExtension[] {};
                Checker.CheckRules(
                    new []
                    {
                        new RuleSrc("S", 
                            new []
                            {
                                new RuleItem(
                                    RuleItemType.VariableName, 
                                    "test1")
                            },
                            extensionType: "test1",
                            extensionQuery: "test")
                    },
                    new Rule[] {},
                    extensions: exts
                );
            });
        }
        
        [Test]
        public void TokenToSplit()
        {
            var exts = new[]
            {
                _createExtensions("test", new[]
                {
                    new Dictionary<string, string>()
                    {
                        {"test", "строка проблелом"}
                    }
                })
            };
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", 
                        new []
                        {
                            new RuleItem(
                                RuleItemType.VariableName, 
                                "test")
                        },
                        extensionType: "test",
                        extensionQuery: "test")
                },
                new []
                {
                    new Rule("S_gen_test_0", new []
                    {
                        new Terminal(condition: new TextCondition("строка")),
                        new Terminal(condition: new TextCondition("проблелом"))
                    }),
                    new Rule("S", new []
                    {
                        new NonTerminal("S_gen_test_0", localName: "test"), 
                    })
                },
                extensions: exts
            );
        }
        
        [Test]
        public void TokenToSplitWithMorph()
        {
            var exts = new[]
            {
                _createExtensions("test", new[]
                {
                    new Dictionary<string, string>()
                    {
                        {"test", "ставка верховного главнокомандования"}
                    }
                })
            };
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", 
                        new []
                        {
                            new RuleItem(
                                RuleItemType.VariableName, 
                                "test",
                                conditions: new []
                                {
                                    new Condition("lemma", false)
                                })
                        },
                        extensionType: "test",
                        extensionQuery: "test")
                },
                new []
                {
                    new Rule("S_gen_test_0", new []
                    {
                        new Terminal(condition: new LemmaCondition("ставка")),
                        new Terminal(condition: new TextCondition("верховного")),
                        new Terminal(condition: new TextCondition("главнокомандования"))
                    }),
                    new Rule("S", new []
                    {
                        new NonTerminal("S_gen_test_0", localName: "test"), 
                    })
                },
                extensions: exts
            );
        }
        
        
    }
}