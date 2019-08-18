using System.Collections.Generic;
using TxTraktor.Source.Model;
using Rule = TxTraktor.Compile.Model.Rule;
using RuleSrc = TxTraktor.Source.Model.Rule;
namespace TxTraktor.Compile
{
    internal interface IGrammarCompiler
    {
        IEnumerable<Rule> Compile(IEnumerable<Grammar> grammars, Dictionary<string, string> variables=null);
        IEnumerable<Rule> CompileRules(IEnumerable<RuleSrc> srcRls, Dictionary<string, string> variables=null);
    }
}