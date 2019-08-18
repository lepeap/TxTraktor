namespace TxTraktor.Compile.Condition
{
    internal class StartTextCondition : ConditionBase
    {
        public override bool IsValid(Token token)
        {
            return token.Index == 0;
        }
        
                
        public class Provider : ConditionProvider<StartTextCondition>
        {
            public override ConditionArgType ArgType => ConditionArgType.None;
            public override string[] Keys => new[] {"начало", "start"};
        }
    }
}