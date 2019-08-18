using TxTraktor.Compile.Condition;

namespace TxTraktor.Compile.Model
{
    internal class NonTerminal : TermBase
    {
        public NonTerminal(string name,
                           bool isNullable = false, 
                           string localName = null, 
                           ICondition condition = null,
                           bool isHead = false
            ) : 
            base(false, isNullable, localName, condition, isHead, name)
        {
        }

        public string Name => _Name;
        public Rule[] Rules { get;  set; }

        public override string ToString()
        {
            return Name;
        }
    }
}