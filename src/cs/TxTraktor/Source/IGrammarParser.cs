using TxTraktor.Source.Model;

namespace TxTraktor.Source
{
    internal interface IGrammarParser
    {
        Grammar Parse(string grammarKey, string text);
    }

}