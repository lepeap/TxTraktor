using System.IO;
using Antlr4.Runtime;

namespace TxTraktor.Source
{
    internal class ErrorListener : BaseErrorListener
    {
        private ILogger _logger;
        private string _grammarKey;
        public ErrorListener(string grammarKey, ILogger logger)
        {
            _grammarKey = grammarKey;
            _logger = logger;
        }

        public bool HasErrors { get; private set; }

        public override void SyntaxError(TextWriter output, 
            IRecognizer recognizer, 
            IToken offendingSymbol, 
            int line, 
            int charPositionInLine,
            string msg, 
            RecognitionException e)
        {
            HasErrors = true;
            _logger.Error("Syntax error at grammar {GrammarKey}. Message \"{Message}\" at line {Line} position {Position}", 
                _grammarKey, msg, line, charPositionInLine);
        }
    }
}