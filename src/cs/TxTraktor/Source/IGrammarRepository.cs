using System.Collections.Generic;

namespace TxTraktor.Source
{
    public interface IGrammarRepository
    {
        IEnumerable<(string key, string src)> GetAll();
    }
}