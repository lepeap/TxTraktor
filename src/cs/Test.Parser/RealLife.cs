using NUnit.Framework;
using TxTraktor;
using TxTraktor.Compile.Condition;
using TxTraktor.Compile.Model;
using TxTraktor.Morphology;

namespace TxtTractor.Test.Parser
{
    [Parallelizable(ParallelScope.All)]
    public class RealLife
    {
        [Test]
        public void Date1()
        {
            var rules = new[]
            {
                new Rule(
                    "S",
                    new[]
                    {
                        new Terminal(condition: new RegexCondition(@"\d?\d")),
                        new Terminal(condition: new LemmaCondition("июль")),
                        new Terminal(condition: new RegexCondition(@"\d\d\d\d")),
                        new Terminal(condition: new TextCondition("г")),
                        new Terminal(condition: new TextCondition(".")),
                    },
                    isStart: true
                )
            };
            var tokens = new[]
            {
                new Token("3"),
                new Token("июля"){Morphs = new []{new MorphInfo("июль", null), }},
                new Token("1941"),
                new Token("г"),
                new Token(".")
            };
            var finalStates = new[]
            {
                new FinalState("3 июля 1941 г .", "S", 0, 5), 
            };
            Checker.Check(rules, tokens, finalStates);
        }
    }
}