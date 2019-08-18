using NUnit.Framework;
using TxTraktor.Source.Model;

namespace TxtTractor.Test.Source
{
    [Parallelizable(ParallelScope.All)]
    public class Head
    {
        [Test]
        public void NonTeminalHead()
        {
            Checker.CheckRule(
                "S -> !T \"хер\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T", isHead: true),
                            new RuleItem(RuleItemType.Terminal, "хер")
                        })
                }
            );
        }
    }
}