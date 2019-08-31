using System.Collections.Generic;

namespace TxTraktor.Tokenize
{
    public interface ITokenizer
    {
        IEnumerable<Token> Tokenize(string text);
    }
}