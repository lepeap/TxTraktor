namespace TxTraktor.Compile.Condition
{
    internal interface IConditionProvider
    {
        string[] Keys { get; }
        
        ICondition Create(string[] args);
    }
}