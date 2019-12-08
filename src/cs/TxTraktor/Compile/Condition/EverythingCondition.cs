namespace TxTraktor.Compile.Condition
{
    internal class EverythingCondition : ConditionBase
    {
        public override bool IsValid(Token token)
        {
            return true;
        }
    }
}