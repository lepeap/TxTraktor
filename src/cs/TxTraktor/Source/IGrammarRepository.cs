using System.Collections.Generic;

namespace TxTraktor.Source
{
    public interface IGrammarRepository
    {
        IEnumerable<string> ListAll();
        string Get(string key);
        void Save(string key, string text);
        IEnumerable<(string key, string src)> GetAll();
    }
}