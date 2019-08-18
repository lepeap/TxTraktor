using NUnit.Framework;
using TxTraktor.Source.Model;
using TxTraktor.Source.Model.Extraction;

namespace TxtTractor.Test.Source
{
    [Parallelizable(ParallelScope.All)]
    public class StaticVars
    {
        [Test]
        public void StaticVarInt()
        {
            Checker.CheckRule(
                "S[Value=$val] -> T l\"хер\" #set val=123",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T"),
                            new RuleItem(RuleItemType.Lemma, "хер")
                        },
                        template: new Template(items: new TemplateItemBase[]
                        {
                            new TemplateItem<(string, string)>("Value", ("val", null), TemplateValueType.NameRef), 
                        }),
                        staticVars:  new TemplateItemBase[]{
                            new TemplateItem<int>("val", 123, TemplateValueType.Integer)
                        }
                    )
                        
                }
            );
        }
        
        [Test]
        public void StaticVarString()
        {
            Checker.CheckRule(
                "S[Value=$val] -> T l\"хер\"\n #set val=\"123\"",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T"),
                            new RuleItem(RuleItemType.Lemma, "хер")
                        },
                        template: new Template(items: new TemplateItemBase[]
                        {
                            new TemplateItem<(string, string)>("Value", ("val", null), TemplateValueType.NameRef), 
                        }),
                        staticVars:  new TemplateItemBase[]{
                            new TemplateItem<string>("val", "123", TemplateValueType.String)
                        }
                    )
                        
                }
            );
        }
        
        [Test]
        public void StaticVarBool()
        {
            Checker.CheckRule(
                "S[Value=$val] -> T l\"хер\" #set val=true",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T"),
                            new RuleItem(RuleItemType.Lemma, "хер")
                        },
                        template: new Template(items: new TemplateItemBase[]
                        {
                            new TemplateItem<(string, string)>("Value", ("val", null), TemplateValueType.NameRef), 
                        }),
                        staticVars:  new TemplateItemBase[]{
                            new TemplateItem<bool>("val", true, TemplateValueType.Bool)
                        }
                    )
                        
                }
            );
        }
        
        [Test]
        public void StaticVarFloat()
        {
            Checker.CheckRule(
                "S[Value=$val] -> T l\"хер\"\n #set val=12.12",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.NonTerminal, "T"),
                            new RuleItem(RuleItemType.Lemma, "хер")
                        },
                        template: new Template(items: new TemplateItemBase[]
                        {
                            new TemplateItem<(string, string)>("Value", ("val", null), TemplateValueType.NameRef), 
                        }),
                        staticVars:  new TemplateItemBase[]{
                            new TemplateItem<float>("val", (float)12.12, TemplateValueType.Float)
                        }
                    )
                        
                }
            );
        }
    }
}