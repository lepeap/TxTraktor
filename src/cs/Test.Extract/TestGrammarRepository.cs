using System.Collections.Generic;
using TxTraktor.Source;

namespace Test.Extract
{
    public class TestGrammarRepository : IGrammarRepository
    {
        private string _grammar;
        public TestGrammarRepository(string rules)
        {
            _grammar = "grammar Test;\nlang ru;\n" + rules;
            
        }
        
        public IEnumerable<(string key, string src)> GetAll()
        {
            yield return (key: "Test", src: _grammar);
        }
    }
}