
using System.Linq;

namespace TxTraktor.Compile.Condition
{
    internal class TextCondition : ConditionBase
    {
        private string _text;

        public TextCondition()
        {
            
        }

        public override void Init(string[] args)
        {
            _text = args.First();
        }

        public TextCondition(string text)
        {
            _text = text;
        }
        public override bool IsValid(Token token)
        {
            return _text == token.LowerText;
        }

        public override string ToString()
        {
            return $"t:{_text}";
        }
    }
}