namespace TxTraktor.Compile.Condition
{
    internal class EndTextCondition : ConditionBase
    {
        public override bool IsValid(Token token)
        {
            return token.TextInfo.TokensCount - 1 == token.Index;
        }

        public class Provider : ConditionProvider<EndTextCondition>
        {
            public override ConditionArgType ArgType => ConditionArgType.None;
            public override string[] Keys => new[] {"конец", "end"};
        }
    }
}