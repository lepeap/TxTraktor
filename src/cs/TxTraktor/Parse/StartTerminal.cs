using System.Collections.Generic;
using System.Linq;
using TxTraktor.Compile.Model;

namespace TxTraktor.Parse
{
    internal class StartTerminal
    {
        private IEnumerable<Rule> _rules;
        public StartTerminal(Terminal terminal, IEnumerable<Rule> rules)
        {
            Terminal = terminal;
            _rules = rules;
        }
        public Terminal Terminal { get; private set; }
        public IEnumerable<Rule> Rules => _rules;


        public bool Check(Token t)
        {
            return Terminal.IsValid(t);
        }

        public static IEnumerable<StartTerminal> Create(IEnumerable<Rule> rules)
        {
            var termDic = new Dictionary<Terminal, List<Rule>>();
            foreach (var rule in rules)
            {
                foreach (var terminal in GetTerminals(rule))
                {
                    if (termDic.ContainsKey(terminal))
                    {
                        termDic[terminal].Add(rule);
                    }
                    else
                    {
                        termDic[terminal] = new List<Rule>(){rule};
                    }
                }
            }

            return termDic.Select(kp => new StartTerminal(kp.Key, kp.Value));
        }


        private static IEnumerable<Terminal> GetTerminals(Rule rule)
        {
            var term = rule.Terms.First();
            if (term.IsTerminal)
            {
                yield return (Terminal) term;
            }
            else
            {
                var nonTerm = (NonTerminal) term;
                foreach (var nonTermRule in nonTerm.Rules)
                {
                    foreach (var terminal in GetTerminals(nonTermRule))
                    {
                        yield return terminal;
                    }
                }
            }
        }
    }
}