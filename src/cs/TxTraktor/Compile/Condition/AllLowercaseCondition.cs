using System.Linq;

namespace TxTraktor.Compile.Condition
{
    internal class AllLowercaseCondition : ConditionBase
    {
        public override bool IsValid(Token token)
        {
            return token.Text.All(char.IsLower);
        }

        public class Provider : ConditionProvider<AllLowercaseCondition>
        {
            public override ConditionArgType ArgType => ConditionArgType.None;
            public override string[] Keys => new[] {"вмал", "asmall"};
        }
    }
}