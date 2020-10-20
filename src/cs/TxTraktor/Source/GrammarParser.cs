using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Microsoft.Extensions.Logging;
using TxTraktor.Source.Listener;
using TxTraktor.Source.Model;

namespace TxTraktor.Source
{
    internal class GrammarParser : IGrammarParser
    {
        private ILogger<IGrammarParser> _logger;
        public GrammarParser(ILogger<IGrammarParser> logger)
        {
            _logger = logger;
        }
        
        public Grammar Parse(string grammarKey, string text)
        {
            var inputStream = new AntlrInputStream(text);
            var lexer = new CfgGramLexer(inputStream);
            var tStream = new CommonTokenStream(lexer);
            var parser = new CfgGramParser(tStream);
            parser.RemoveErrorListeners();
            var errorListener = new ErrorListener(grammarKey, _logger);
            parser.AddErrorListener(errorListener);

            var cont = parser.gram();
            ParseTreeWalker walker = new ParseTreeWalker();
            var listener = new Main();
            walker.Walk(listener, cont);

            if (errorListener.HasErrors)
            {
                return new Grammar()
                {
                    Key = grammarKey,
                    Errors = errorListener.Errors.ToArray()
                };
            }

            return listener.Grammar;
        }
    }
}