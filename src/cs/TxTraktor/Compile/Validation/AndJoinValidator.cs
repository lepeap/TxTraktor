using System.Collections.Generic;
using System.Linq;
using System.Text;
using TxTraktor.Parse.Forest;

namespace TxTraktor.Compile.Validation
{
    internal class AndJoinValidator : IValidator
    {
        private IValidator[] _validators;
        public AndJoinValidator(IEnumerable<IValidator> validators)
        {
            _validators = validators.ToArray();
        }
        
        public bool Validate(IEnumerable<NodeBase> nodes)
        {
            foreach (var validator in _validators)
            {
                if (!validator.Validate(nodes))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var validator in _validators)
            {
                sb.Append("(");
                sb.Append(validator);
                sb.Append(") &");
            }

            return sb.ToString();
        }
    }
}