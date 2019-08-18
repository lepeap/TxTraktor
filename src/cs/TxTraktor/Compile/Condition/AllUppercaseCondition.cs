using System.Linq;

namespace TxTraktor.Compile.Condition
{
    internal class AllUppercaseCondition : ConditionBase
    {
        public override bool IsValid(Token token)
        {
            return token.Text.All(char.IsUpper);
        }
        
        public class Provider : ConditionProvider<AllUppercaseCondition>
        {
            public override ConditionArgType ArgType => ConditionArgType.None;
            public override string[] Keys => new[] {"вбол", "abig"};
        }
    }
}