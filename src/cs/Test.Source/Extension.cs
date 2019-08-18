using NUnit.Framework;
using TxTraktor.Source.Model;

namespace TxtTractor.Test.Source
{
    [Parallelizable(ParallelScope.All)]
    public class Extension
    {
        [Test]
        public void Simple()
        {
            Checker.CheckRule(
                "S -> "
                      +"<spql>query</end>"
                      +"r\"123\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Regex, "123")
                        },
                        extensionType:"spql",
                        extensionQuery:"query")
                }
            );
        }
        
        [Test]
        public void MultiLineQuery()
        {
            Checker.CheckRule(
                "S -> "
                +"<spql>query\n123</end>"
                +"r\"123\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Regex, "123")
                        },
                        extensionType:"spql",
                        extensionQuery:"query\n123")
                }
            );
        }
    }
}