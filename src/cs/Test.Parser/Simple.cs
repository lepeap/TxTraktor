using NUnit.Framework;
using TxTraktor;
using TxTraktor.Compile.Condition;
using TxTraktor.Compile.Model;

namespace TxtTractor.Test.Parser
{
    [Parallelizable(ParallelScope.All)]
    public class Simple
    {
        [Test]
        public void OneRuleOneTerminal()
        {
            var rules = new[]
            {
                new Rule(
                    "S",
                    new[]
                    {
                        new Terminal(condition: new TextCondition("123"))
                    },
                    isStart: true
                )
            };
            var tokens = new[]
            {
                new Token("123")
            };
            var finalStates = new[]
            {
                new FinalState("123", "S", 0, 1), 
            };
            Checker.Check(rules, tokens, finalStates);
        }
        
        [Test]
        public void OneRuleTwoTerminal()
        {
            var rules = new[]
            {
                new Rule(
                    "S",
                    new[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("234"))
                    },
                    isStart: true
                )
            };
            var tokens = new[]
            {
                new Token("123"),
                new Token("234"),

            };
            var finalStates = new[]
            {
                new FinalState("123 234", "S", 0, 2), 
            };
            Checker.Check(rules, tokens, finalStates);
        }
        
        [Test]
        public void OneRuleTwoTerminalInText()
        {
            var rules = new[]
            {
                new Rule(
                    "S",
                    new[]
                    {
                        new Terminal(condition: new TextCondition("123")),
                        new Terminal(condition: new TextCondition("234"))
                    },
                    isStart: true
                )
            };
            var tokens = new[]
            {
                new Token("33"),
                new Token("123"),
                new Token("234"),
                new Token("45")

            };
            var finalStates = new[]
            {
                new FinalState("123 234", "S", 1, 3), 
            };
            Checker.Check(rules, tokens, finalStates);
        }
        
        [Test]
        public void OneRuleOneRegTerminal()
        {
            var rules = new[]
            {
                new Rule(
                    "S",
                    new[]
                    {
                        new Terminal(condition: new RegexCondition("123.*"))
                    },
                    isStart: true
                )
            };
            var tokens = new[]
            {
                new Token("12345")
            };
            var finalStates = new[]
            {
                new FinalState("12345", "S", 0, 1), 
            };
            Checker.Check(rules, tokens, finalStates);
        }
        
        [Test]
        public void TwoRulesWithNonTerminal1()
        {
            var nonTermRule = new Rule("S1",
                new []
                {
                    new Terminal(condition: new TextCondition("1234"))
                });
            var rules = new[]
            {
                nonTermRule,
                new Rule(
                    "S",
                    new TermBase[]
                    {
                        new NonTerminal("S1"){Rules=new[]{nonTermRule}}, 
                        new Terminal(condition: new TextCondition(".")),
                    },
                    isStart: true
                )
            };
            var tokens = new[]
            {
                new Token("тест"),
                new Token("1234"),
                new Token(".")
            };

            var finalStates = new[]
            {
                new FinalState("1234", "S1", 1, 2), 
                new FinalState("1234 .", "S", 1, 3), 
            };
            Checker.Check(rules, tokens, finalStates);
        }
                
        [Test]
        public void TwoRulesWithNonTerminal2()
        {
            var nonTermRule = new Rule("BigCir",
                                        new []
                                        {
                                            new Terminal(condition: new RegexCondition("[А-Я]"))
                                        });
            var rules = new[]
            {
                nonTermRule,
                new Rule(
                    "S",
                    new TermBase[]
                    {
                        new NonTerminal("BigCir"){Rules=new[]{nonTermRule}}, 
                        new Terminal(condition: new TextCondition(".")),
                        new Terminal(condition: new RegexCondition("[А-Я][а-я]+"))
                    },
                    isStart: true
                )
            };
            var tokens = new[]
            {
                new Token("тов"),
                new Token("."),
                new Token("И"),
                new Token("."),
                new Token("Сталин")
            };

            var finalStates = new[]
            {
                new FinalState("И", "BigCir", 2, 3), 
                new FinalState("И . Сталин", "S", 2, 5), 
            };
            Checker.Check(rules, tokens, finalStates);
        }
        
        [Test]
        public void TwoRulesWithOverlapNonTerminal()
        {
            var nonTermRule = new Rule("BigCir",
                new []
                {
                    new Terminal(condition: new RegexCondition("[А-Я]"))
                });
            var rules = new[]
            {
                nonTermRule,
                new Rule(
                    "S",
                    new TermBase[]
                    {
                        new NonTerminal("BigCir"){Rules=new[]{nonTermRule}}, 
                        new Terminal(condition: new TextCondition(".")),
                        new Terminal(condition: new RegexCondition("[А-Я][а-я]+"))
                    },
                    isStart: true
                ),
                new Rule(
                    "S",
                    new TermBase[]
                    {
                        new NonTerminal("BigCir"){Rules=new[]{nonTermRule}}, 
                        new Terminal(condition: new TextCondition(".")),
                        new NonTerminal("BigCir"){Rules=new[]{nonTermRule}}, 
                        new Terminal(condition: new TextCondition(".")),
                        new Terminal(condition: new RegexCondition("[А-Я][а-я]+"))
                    },
                    isStart: true
                )
            };
            var tokens = new[]
            {
                new Token("тов"),
                new Token("."),
                new Token("И"),
                new Token("."),
                new Token("В"),
                new Token("."),
                new Token("Сталин")
            };

            var finalStates = new[]
            {
                new FinalState("И", "BigCir", 2, 3),
                new FinalState("И . В . Сталин", "S", 2, 7), 
                new FinalState("В", "BigCir", 4, 5),
                new FinalState("В . Сталин", "S", 4, 7), 
            };
            Checker.Check(rules, tokens, finalStates);
        }
        
        [Test]
        public void EverythingWith2Terminals()
        {
            var rules = new[]
            {
                new Rule(
                    "S",
                    new[]
                    {
                        new Terminal(condition: new TextCondition("1")),
                        new Terminal(condition: new EverythingCondition()),
                        new Terminal(condition: new TextCondition("3"))
                    },
                    isStart: true
                )
            };
            var tokens = new[]
            {
                new Token("1"),
                new Token("2"),
                new Token("3"),
                new Token("4"),

            };
            var finalStates = new[]
            {
                new FinalState("1 2 3", "S", 0, 3), 
            };
            Checker.Check(rules, tokens, finalStates);
        }
    }
}