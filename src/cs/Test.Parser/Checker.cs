using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TxTraktor;
using TxTraktor.Compile.Model;
using TxTraktor.Parse;

namespace TxtTractor.Test.Parser
{
    internal static class Checker
    {
        private static Chart _createChart(IEnumerable<Rule> rules, IEnumerable<Token> tokens)
        {
            var sRls = StartTerminal.Create(rules.Where(x => x.IsStart)).ToArray();
            var parser = new EarleyParser(sRls, null);
            return parser.Parse(tokens);
        }
        public static void Check(IEnumerable<Rule> rules, IEnumerable<Token> tokens, FinalState[] etalonItems)
        {
            var resultChart = _createChart(rules, tokens);

            resultChart = resultChart.GetOnyCompletedChart();
            var resultStates = resultChart.FlatStates
                                          .ToArray();
            
            Assert.AreEqual(etalonItems.Length,
                            resultStates.Length,
                            "Wrong states count");

            for (int i = 0; i < etalonItems.Length; i++)
            {
                var etState = etalonItems[i];
                var rezState = resultStates[i];
                
                Assert.AreEqual(etState.Text,
                                resultChart.GetTokensText(rezState.StartColumnIndex, rezState.EndColumnIndex.Value),
                                "Wrong state text");
                
                Assert.AreEqual(etState.StartColumn,
                                rezState.StartColumnIndex,
                                "Wrong start column");
                
                Assert.AreEqual(etState.EndColumn,
                                rezState.EndColumnIndex,
                                "Wrong end column");
                
                Assert.AreEqual(etState.RuleName,
                    rezState.Rule.Name,
                    "Wrong rule name");


                
            }
        }

        
    }
}