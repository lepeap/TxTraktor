using System.Collections.Generic;

namespace TxTraktor.Source
{
    internal interface IGrammarRepository
    {
        IEnumerable<(string key, string src)> GetAll();
    }
}