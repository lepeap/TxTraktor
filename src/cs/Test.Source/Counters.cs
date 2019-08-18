using NUnit.Framework;
using TxTraktor.Source.Model;

namespace TxtTractor.Test.Source
{    
    [Parallelizable(ParallelScope.All)]
    public class Counters
    {
        [Test]
        public void NonTerminalWithStar()
        {
            Checker.CheckRule(
                "S -> T*",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", counter: Counter.Star)
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalWithPlus()
        {
            Checker.CheckRule(
                "S -> T+",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", counter: Counter.Plus)
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalWithQuestion()
        {
            Checker.CheckRule(
                "S -> T?",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", counter: Counter.Question)
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalWithNumberDefault()
        {
            Checker.CheckRule(
                "S -> T{,3}",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, 
                                         "T", 
                                          counter: Counter.Number,
                                          counterValue: new CounterValue(0, 3)
                                )
                        })
                }
            );
        }
        
        [Test]
        public void NonTerminalWithNumbersFull()
        {
            Checker.CheckRule(
                "S -> T{1,3}",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, 
                                "T", 
                                counter: Counter.Number,
                                counterValue: new CounterValue(1, 3)
                            )
                        })
                }
            );
        }
    }
}