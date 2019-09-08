using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Errors = new List<string>();
        }
        
        public  List<string> Errors { get; }

        public bool HasErrors => Errors.Any();

        public override void SyntaxError(TextWriter output, 
            IRecognizer recognizer, 
            IToken offendingSymbol, 
            int line, 
            int charPositionInLine,
            string msg, 
            RecognitionException e)
        {
            var error =
                $"Syntax error at grammar \"{_grammarKey}\". Message: \"{msg}\" at line {line} position {charPositionInLine}";
            Errors.Add(error);
            _logger.Error(error);
        }
    }
}