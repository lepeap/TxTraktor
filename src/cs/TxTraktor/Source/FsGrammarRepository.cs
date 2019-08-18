using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TxTraktor.Source
{
    internal class FsGrammarRepository : IGrammarRepository
    {
        private readonly string _grammarDir;
        private readonly string _pattern;
        public FsGrammarRepository(string grammarDir, string fileExt)
        {
            _grammarDir = grammarDir;
            _pattern = $"*.{fileExt}";
        }
        
        public IEnumerable<(string key, string src)> GetAll()
        {
            return Directory.GetFiles(_grammarDir, _pattern, SearchOption.AllDirectories)
                            .Select(f=> (key: f, src: File.ReadAllText(f)));
        }
    }
}