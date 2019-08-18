using TxTraktor.Compile.Condition;

namespace TxTraktor.Compile.Model
{
    internal class Terminal : TermBase
    {
        public Terminal(string localName = null, ICondition condition = null, bool isHead = false, string semanticId=null) 
            : base(true, false, localName, condition, isHead, semanticId: semanticId)
        {
        }

        public override string ToString()
        {
            return $"'{Condition}'";
        }
    }
}