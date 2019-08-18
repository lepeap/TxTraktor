namespace TxTraktor.Compile.Condition
{
    internal abstract class ConditionProvider<T>  : IConditionProvider where T : ICondition, new()
    {
        public abstract ConditionArgType ArgType { get; }
        public abstract string[] Keys { get; }
        
        public ICondition Create(string[] args)
        {
            var condition = new T();
            if (ArgType == ConditionArgType.String && (args==null || args.Length != 1))
            {
                throw new WrongConditionArgsException(
                    $"Wrong argument type for condition '{typeof(T)}' expected type '{ArgType}'");
            }
            if (ArgType == ConditionArgType.Array && (args==null || args.Length == 0))
            {
                throw new WrongConditionArgsException(
                    $"Wrong argument type for condition '{typeof(T)}' expected type '{ArgType}'");
            }
            condition.Init(args);
            return condition;
        }
        
    }
}