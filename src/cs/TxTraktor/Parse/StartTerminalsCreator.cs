using System.Collections.Generic;
using System.Linq;
using TxTraktor.Compile.Model;

namespace TxTraktor.Parse
{
    internal class StartTerminalsCreator : IStartTerminalsCreator
    {
        private ExtractorSettings _settings;
        public StartTerminalsCreator(ExtractorSettings settings)
        {
            _settings = settings;
        }
        public IEnumerable<StartTerminal> Create(IEnumerable<Rule> rls)
        {
            var rules = rls.Where(x =>
                x.HasTemplate && (_settings.RulesToExtract == null || _settings.RulesToExtract.Contains(x.Name)));
            var startItems = StartTerminal.Create(rules).ToArray();
            if (!startItems.Any())
                throw new ExtractionException("No start rules found");
            return startItems;
        }
    }
}