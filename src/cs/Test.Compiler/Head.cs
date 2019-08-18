using NUnit.Framework;
using TxTraktor.Compile.Condition;
using TxTraktor.Compile.Model;
using TxTraktor.Source.Model;
using RuleSrc = TxTraktor.Source.Model.Rule;
using Rule = TxTraktor.Compile.Model.Rule;

namespace TxtTractor.Test.Compiler
{
    [Parallelizable(ParallelScope.All)]
    public class Head
    {
        [Test]
        public void TerminalHead()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []{new RuleItem(RuleItemType.Terminal, "123", isHead: true) })
                },
                new []
                {
                    new Rule("S", new []{new Terminal(condition: new TextCondition("123"), isHead: true) })
                }
            );
        }
        
        [Test]
        public void NonTerminalHead()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.NonTerminal, "S", isHead: true),
                        new RuleItem(RuleItemType.Terminal, "123")
                    })
                },
                new []
                {
                    new Rule("S", new TermBase[]
                    {
                        new NonTerminal("S", isHead: true),
                        new Terminal(condition: new TextCondition("123"))
                    })
                    
                }
            );
        }
    }
}