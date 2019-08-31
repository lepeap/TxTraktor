using System.Collections.Generic;
using TxTraktor;
using TxTraktor.Extension;
using TxTraktor.Source;

namespace Test.Extract
{
    internal class TestExtractorFactory : ExtractorFactory
    {
        private string _rules;
        public TestExtractorFactory(ExtractorSettings settings, IEnumerable<IExtension> extensions, string rules) 
            : base(settings, extensions)
        {
            _rules = rules;
        }

        public override IGrammarRepository GrammarRepository => new TestGrammarRepository(_rules); 
        
    }
}