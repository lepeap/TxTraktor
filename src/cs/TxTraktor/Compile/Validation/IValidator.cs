using System.Collections.Generic;
using TxTraktor.Parse.Forest;

namespace TxTraktor.Compile.Validation
{
    internal interface IValidator
    {
        bool Validate(IEnumerable<NodeBase> node);
    }
}