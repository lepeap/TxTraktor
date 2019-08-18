
namespace TxTraktor.Compile.Condition
{
    internal interface ICondition
    {
        void Init(string[] args);
        
        bool IsValid(Token token);
    }
}