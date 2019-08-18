using NUnit.Framework;
using TxTraktor.Source.Model;
using TxTraktor.Source.Model.Extraction;

namespace TxtTractor.Test.Source
{
    [Parallelizable(ParallelScope.All)]
    public class All
    {
        [Test]
        public void NonTerminalWithCounterAndConditions()
        {
            Checker.CheckRule(
                "S -> T*<cond1=value1;cond2=value2;cond3=123>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, 
                                "T",
                                conditions: new []
                                {
                                    new Condition("cond1","value1"),
                                    new Condition("cond2","value2"),
                                    new Condition("cond3","123"),
                
                                },
                                counter: Counter.Star
                            )   
                        }
                    )
                }
            );
        }
        
        [Test]
        public void NonTerminalWithCounterAndConditionsAndTerminal()
        {
            Checker.CheckRule(
                "S -> T*<cond1=value1;cond2=value2> \"123\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, 
                                "T",
                                conditions: new []
                                {
                                    new Condition("cond1","value1"),
                                    new Condition("cond2","value2")

                                },
                                counter: Counter.Star
                            ),
                            new RuleItem(
                                RuleItemType.Terminal, 
                                "123"
                            )   
                        }
                    )
                }
            );
        }
        
        [Test]
        public void MultipleRules()
        {
            Checker.CheckRule(
                "S -> T*<cond1=value1;cond2=value2> \"123\" | G+<morf=123>",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, 
                                "T",
                                conditions: new []
                                {
                                    new Condition("cond1","value1"),
                                    new Condition("cond2","value2")
//
                                },
                                counter: Counter.Star
                            ),
                            new RuleItem(
                                RuleItemType.Terminal, 
                                "123"
                            )   
                        }
                    ),
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, 
                                "G",
                                conditions: new []
                                {
                                    new Condition("morf","123")
                                },
                                counter: Counter.Plus
                            )
                        }
                    )
                }
            );
        }
        
        [Test]
        public void MultipleRulesWithAll()
        {
            Checker.CheckRule(
                "S[Val1=123123, Val1=$tt.Val] -> T*<cond=val;cond1=valr> as tt",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, 
                                "T",
                                conditions: new []
                                {
                                    new Condition("cond","val"),
                                    new Condition("cond1","valr")

                                },
                                counter: Counter.Star,
                                localName: "tt"
                            )
                        },
                        
                            new Template(
                                new TemplateItemBase[]
                                {
                                    new TemplateItem<int>("Val1", 123123, TemplateValueType.Integer),
                                    new TemplateItem<(string,string)>("Val1", ("tt", "Val"), TemplateValueType.NameRef)
                                }
                            ) 
                        
                    )
                }
            );
        }
        [Test]
        public void MultipleRulesWithAll1()
        {
            var template = new Template(
                new TemplateItemBase[]
                {
                    new TemplateItem<(string, string)>("Val", ("g1", "Name"), TemplateValueType.NameRef),
                    new TemplateItem<(int, string)>("Val2", (1, "Date"), TemplateValueType.NumberRef),
                    new TemplateItem<string>("Val3", "sdf", TemplateValueType.String)
                }
            );
            Checker.CheckRule(
                "S[Val=$g1.Name, Val2=$1.Date, Val3=\"sdf\"] -> T*<cond;cond1=valr;~морф=сущ,прил> as g1 \"123\" | G+<action=123> as g1",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, 
                                "T",
                                conditions: new []
                                {
                                    new Condition("cond", false),
                                    new Condition("cond1","valr"),
                                    new Condition("морф", new[]{"сущ", "прил"}, true),


                                },
                                counter: Counter.Star,
                                localName: "g1"
                            ),
                            new RuleItem(RuleItemType.Terminal,  "123")
                        },
                        template
                    ),
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, 
                                "G",
                                conditions: new []
                                {
                                    new Condition("action","123")
                                },
                                counter: Counter.Plus,
                                localName: "g1"
                            )
                        },
                        template
                    )
                }
            );
        }
    }
}