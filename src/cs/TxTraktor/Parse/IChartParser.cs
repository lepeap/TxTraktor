using System.Collections.Generic;

namespace TxTraktor.Parse
{
    internal interface IChartParser
    {
        Chart Parse(IEnumerable<Token> tokens, params string[] rulesToExtract);
    }
}