using NUnit.Framework;
using TxTraktor.Compile;
using TxTraktor.Compile.Condition;
using TxTraktor.Compile.Model;
using TxTraktor.Compile.Validation;
using TxTraktor.Source.Model;
using RuleSrc = TxTraktor.Source.Model.Rule;
using Rule = TxTraktor.Compile.Model.Rule;

namespace TxtTractor.Test.Compiler
{
    [Parallelizable(ParallelScope.All)]
    public class Validators
    {
        [Test]
        public void SimpleSogl()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.Terminal,
                            "тест",
                                conditions: new []
                                {
                                    new Condition("согл", new []{"ключ", "число"})
                                }
                            ),
                        new RuleItem(RuleItemType.Terminal, 
                            "тест1",
                            conditions: new []
                            {
                                new Condition("согл", new []{"ключ", "число"})
                            })
                    })
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(condition: new TextCondition("тест")),
                        new Terminal(condition: new TextCondition("тест1"))
                    },
                        validator: new SoglValidator("число", new []{0,1}))
                }
            );
        }
        
        [Test]
        public void TwoSogl()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.Terminal,
                            "тест",
                            conditions: new []
                            {
                                new Condition("согл", new []{"ключ", "число"})
                            }
                        ),
                        new RuleItem(RuleItemType.Terminal, 
                            "тест1",
                            conditions: new []
                            {
                                new Condition("согл", new []{"ключ", "число"}),
                                new Condition("согл", new []{"ключ1", "падеж"})
                            }),
                        new RuleItem(RuleItemType.Terminal, 
                        "тест2",
                            conditions: new []
                            {
                                new Condition("согл", new []{"ключ1", "падеж"})
                            })
                    })
                },
                new []
                {
                    new Rule("S", new []
                        {
                            new Terminal(condition: new TextCondition("тест")),
                            new Terminal(condition: new TextCondition("тест1")),
                            new Terminal(condition: new TextCondition("тест2"))
                        },
                        validator: 
                        new AndJoinValidator(new []
                        {
                            new SoglValidator("число", new []{0,1}),
                            new SoglValidator("падеж", new []{1,2}),

                        }))
                        
                }
            );
        }
        
        [Test]
        public void ErrorWrongGrammeme()
        {
            Assert.Throws<CfgCompileException>(() =>
            {
                Checker.CheckRules(
                    new[]
                    {
                        new RuleSrc("S", new[]
                        {
                            new RuleItem(RuleItemType.Terminal,
                                "тест",
                                conditions: new[]
                                {
                                    new Condition("согл", new[] {"ключ", "число"})
                                }
                            ),
                            new RuleItem(RuleItemType.Terminal,
                                "тест1",
                                conditions: new[]
                                {
                                    new Condition("согл", new[] {"ключ", "число"}),
                                    new Condition("согл", new[] {"ключ1", "падеж"})
                                }),
                            new RuleItem(RuleItemType.Terminal,
                                "тест2",
                                conditions: new[]
                                {
                                    new Condition("согл", new[] {"ключ", "падеж"})
                                })
                        })
                    },
                    new Rule[] {}
                );
            });
        }
        
        [Test]
        public void ErrorWrongParamsCount()
        {
            Assert.Throws<CfgCompileException>(() =>
            {
                Checker.CheckRules(
                    new[]
                    {
                        new RuleSrc("S", new[]
                        {
                            new RuleItem(RuleItemType.Terminal,
                                "тест",
                                conditions: new[]
                                {
                                    new Condition("согл", new[] {"ключ", "число"})
                                }
                            ),
                            new RuleItem(RuleItemType.Terminal,
                                "тест1",
                                conditions: new[]
                                {
                                    new Condition("согл", new[] {"ключ", "число", "123"})
                                })
                        })
                    },
                    new Rule[] {}
                );
            });
        }
        
        
        [Test]
        public void ErrorSoglItemsCount()
        {
            Assert.Throws<CfgCompileException>(() =>
            {
                Checker.CheckRules(
                    new[]
                    {
                        new RuleSrc("S", new[]
                        {
                            new RuleItem(RuleItemType.Terminal,
                                "тест",
                                conditions: new[]
                                {
                                    new Condition("согл", new[] {"ключ", "число"})
                                }
                            ),
                            new RuleItem(RuleItemType.Terminal, "тест1")
                        })
                    },
                    new Rule[] {}
                );
            });
        }
    }
}