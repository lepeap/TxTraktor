using System.Collections.Generic;
using System.Linq;

namespace TxTraktor.Compile.Condition
{
    internal class AndCondition : ConditionBase
    {
        private readonly ICondition[] _conds;
        public AndCondition(IEnumerable<ICondition> conds)
        {
            _conds = conds.ToArray();
        }
        public override bool IsValid(Token token)
        {
            return _conds.Select(c => c.IsValid(token))
                         .All(x=>x);
        }
        public override string ToString()
        {
            return string.Concat(_conds.Select(x=>x.ToString()), "&");
        }
    }
}