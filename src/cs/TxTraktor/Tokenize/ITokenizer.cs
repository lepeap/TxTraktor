using System.Collections.Generic;

namespace TxTraktor.Tokenize
{
    internal interface ITokenizer
    {
        IEnumerable<Token> Tokenize(string text);
    }
}