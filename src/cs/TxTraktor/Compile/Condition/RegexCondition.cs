using System.Text.RegularExpressions;

namespace TxTraktor.Compile.Condition
{
    internal class RegexCondition : ConditionBase
    {
        private Regex _reg;
        private string _text;

        public RegexCondition()
        {
            
        }

        public RegexCondition(string regTxt)
        {
            _init(regTxt);
        }
        
        public override void Init(string[] args)
        {
            if (args.Length > 1)
            {
                throw new WrongConditionArgsException("Regex condition cant have multiple args");
            }
            if (args.Length == 0)
            {
                throw new WrongConditionArgsException("Regex condition cant have zero args");
            }
            _init(args[0]);
        }

        private void _init(string regTxt)
        {
            _text = regTxt;
            _reg = new Regex($"^{_text}$", RegexOptions.Compiled);
        }
        public override bool IsValid(Token token)
        {
            return _reg.IsMatch(token.Text);
        }
        
        public override string ToString()
        {
            return $"r:{_text}";
        }
                
        public class Provider : ConditionProvider<RegexCondition>
        {
            public override ConditionArgType ArgType => ConditionArgType.String;
            public override string[] Keys => new[] {"рег", "regex"};
        }
    }
}