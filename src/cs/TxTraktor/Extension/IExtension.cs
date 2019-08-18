using System.Collections.Generic;

namespace TxTraktor.Extension
{
    public interface IExtension
    {
        string Name { get; }
        IEnumerable<Dictionary<string, string>> Process(string query);
    }
}