using System.Collections.Generic;

namespace TxTraktor.Extract
{
    public interface IExtractor
    {
        IEnumerable<ExtractionDic> Parse(string text);
    }
}