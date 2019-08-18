namespace TxTraktor.Compile.Condition
{
    internal class StartsWithUpperCondition :  ConditionBase
    {
        public override bool IsValid(Token token)
        {
            return char.IsUpper(token.Text[0]);
        }
        
        public class Provider : ConditionProvider<StartsWithUpperCondition>
        {
            public override ConditionArgType ArgType => ConditionArgType.None;
            public override string[] Keys => new[] {"нбол", "sbig"};
        }
    }
}