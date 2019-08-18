using System.Collections.Generic;
using NUnit.Framework;
using TxTraktor.Compile;
using TxTraktor.Compile.Condition;
using TxTraktor.Compile.Model;
using TxTraktor.Source.Model;
using RuleSrc = TxTraktor.Source.Model.Rule;
using Rule = TxTraktor.Compile.Model.Rule;

namespace TxtTractor.Test.Compiler
{
    [Parallelizable(ParallelScope.All)]
    public class Variables
    {
        [Test]
        public void OneVariable()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.VariableName, "test")
                    })
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(localName: "test", condition: new TextCondition("123"))
                    })
                },
                variables: new Dictionary<string, string>()
                {
                    {"test", "123"}
                }
            );
        }
        
        [Test]
        public void TwoVariables()
        {
            Checker.CheckRules(
                new []
                {
                    new RuleSrc("S", new []
                    {
                        new RuleItem(RuleItemType.VariableName, "test"),
                        new RuleItem(RuleItemType.VariableName, "var1")
                    })
                },
                new []
                {
                    new Rule("S", new []
                    {
                        new Terminal(localName: "test", condition: new TextCondition("123")),
                        new Terminal(localName: "var1", condition: new TextCondition("variable value"))
                    })
                },
                variables: new Dictionary<string, string>()
                {
                    {"test", "123"},
                    {"var1", "variable value"}
                }
            );
        }
        
        [Test]
        public void UndefinedVariable1()
        {
            Assert.Throws<CfgCompileException>(() =>
            {
                Checker.CheckRules(
                    new []
                    {
                        new RuleSrc("S", new []
                        {
                            new RuleItem(RuleItemType.VariableName, "test")
                        })
                    },
                    new Rule[] { }
                );
            });
            
        }
        
                
        [Test]
        public void UndefinedVariable2()
        {
            Assert.Throws<CfgCompileException>(() =>
            {
                Checker.CheckRules(
                    new []
                    {
                        new RuleSrc("S", new []
                        {
                            new RuleItem(RuleItemType.VariableName, "test")
                        })
                    },
                    new Rule[] { },
                    variables: new Dictionary<string, string>()
                    {
                        {"123", "123"}
                    }
                );
            });
            
        }
    }
}