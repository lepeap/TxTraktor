namespace TxTraktor.Compile.Condition
{
    internal abstract class ConditionBase : ICondition
    {
        public virtual void Init(string[] args)
        {
        }

        public abstract bool IsValid(Token token);
    }
}