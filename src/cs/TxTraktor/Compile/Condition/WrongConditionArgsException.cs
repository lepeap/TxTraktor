namespace TxTraktor.Compile.Condition
{
    public class WrongConditionArgsException : ExtractionException
    {
        public WrongConditionArgsException(string message) : base(message)
        {
        }
    }
}