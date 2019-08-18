using System.Collections.Generic;
using TxTraktor.Compile.Model;

namespace TxTraktor.Parse
{
    internal interface IStartTerminalsCreator
    {
        IEnumerable<StartTerminal> Create(IEnumerable<Rule> rls);
    }
}