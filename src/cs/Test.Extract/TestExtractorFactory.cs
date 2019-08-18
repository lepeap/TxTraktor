using TxTraktor;
using TxTraktor.Source;

namespace Test.Extract
{
    internal class TestExtractorFactory : ExtractorFactory
    {
        private string _rules;
        public TestExtractorFactory(string rules)
        {
            _rules = rules;
        }

        internal override IGrammarRepository _CreateGrammarRepository(ExtractorSettings settings)
        {
            return new TestGrammarRepository(_rules);
        }


    }
}