using NUnit.Framework;
using TxTraktor.Source.Model;
using TxTraktor.Source.Model.Extraction;

namespace TxtTractor.Test.Source
{
    [Parallelizable(ParallelScope.All)]
    public class Templates
    {
        [Test]
        public void IntValue()
        {
            Checker.CheckTemplate(
                "Val1=123123",
                new Template(
                    new[]
                    {
                        new TemplateItem<int>("Val1", 123123, TemplateValueType.Integer)
                    }
                )
            );
        }

        [Test]
        public void FloatValue()
        {
            Checker.CheckTemplate(
                "Val1=123.123",
                new Template(
                    new[]
                    {
                        new TemplateItem<float>("Val1", (float) 123.123, TemplateValueType.Float)
                    }
                )
            );
        }

        [Test]
        public void BoolValue()
        {
            Checker.CheckTemplate(
                "Val1=true",
                new Template(
                    new[]
                    {
                        new TemplateItem<bool>("Val1", true, TemplateValueType.Bool)
                    }
                )
            );
        }

        [Test]
        public void StringValue()
        {
            Checker.CheckTemplate(
                "Val1=\"true\"",
                new Template(
                    new[]
                    {
                        new TemplateItem<string>("Val1", "true", TemplateValueType.String)
                    }
                )
            );
        }
        
        [Test]
        public void NameRefValue()
        {
            Checker.CheckTemplate(
                "Val1=$test",
                new Template(
                    new[]
                    {
                        new TemplateItem<(string, string)>("Val1", ("test", null), TemplateValueType.NameRef)
                    }
                )
            );
        }

        [Test]
        public void NameRefFullValue()
        {
            Checker.CheckTemplate(
                "Val1=$test.Name",
                new Template(
                    new[]
                    {
                        new TemplateItem<(string, string)>("Val1", ("test", "Name"), TemplateValueType.NameRef)
                    }
                )
            );
        }
        
        [Test]
        public void NumberRefValue()
        {
            Checker.CheckTemplate(
                "Val1=$1", 
                new Template(
                    new[]
                    {
                        new TemplateItem<(int, string)>("Val1", (1, null), TemplateValueType.NumberRef)
                    }
                )
            );
        }

        [Test]
        public void NumberRefFullValue()
        {
            Checker.CheckTemplate(
                "Val1=$1.Name", 
                new Template(
                    new[]
                    {
                        new TemplateItem<(int, string)>("Val1", (1, "Name"), TemplateValueType.NumberRef)
                    }
                )
            );
        }
        
        [Test]
        public void ListValueBoolInt()
        {
            Checker.CheckTemplate(
                "Val1={true,123}",
                new Template(
                    new[]
                    {
                        new TemplateItem<TemplateItemBase[]>(
                            "Val1", 
                            new TemplateItemBase[]
                                  {
                                      new TemplateItem<bool>(null, true, TemplateValueType.Bool), 
                                      new TemplateItem<int>(null, 123, TemplateValueType.Integer)
                                  }, 
                            TemplateValueType.List)
                    }
                )
            );
        }
        [Test]
        public void ListValueBoolIntFloat()
        {
            Checker.CheckTemplate(
                "Val1={true,123,0.001}",
                new Template(
                    new[]
                    {
                        new TemplateItem<TemplateItemBase[]>(
                            "Val1", 
                            new TemplateItemBase[]
                            {
                                new TemplateItem<bool>(null, true, TemplateValueType.Bool), 
                                new TemplateItem<int>(null, 123, TemplateValueType.Integer),
                                new TemplateItem<float>(null, (float)0.001, TemplateValueType.Float)
                            }, 
                            TemplateValueType.List)
                    }
                )
            );
        }
        
        [Test]
        public void ListValue()
        {
            Checker.CheckTemplate(
                "Val1={true, 123, 0.001}, Val2={$name.Test, $0.Test1}",
                new Template(
                    new[]
                    {
                        new TemplateItem<TemplateItemBase[]>(
                            "Val1", 
                            new TemplateItemBase[]
                            {
                                new TemplateItem<bool>(null, true, TemplateValueType.Bool), 
                                new TemplateItem<int>(null, 123, TemplateValueType.Integer),
                                new TemplateItem<float>(null, (float)0.001, TemplateValueType.Float)
                            }, 
                            TemplateValueType.List),
                        new TemplateItem<TemplateItemBase[]>(
                            "Val2", 
                            new TemplateItemBase[]
                            {
                                new TemplateItem<(string, string)>(null, ("name", "Test"), TemplateValueType.NameRef), 
                                new TemplateItem<(int, string)>(null, (0, "Test1"), TemplateValueType.NumberRef)
                            }, 
                            TemplateValueType.List)
                    }
                )
            );
        }
        
        
        [Test]
        public void DefaultTemplateOneValue()
        {
            Checker.CheckRule(
                "S -> \"123\" as value",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "123", localName: "value")
                        },
                        new Template(new []
                        {
                            new TemplateItem<(string, string)>("Value", ("value", null), TemplateValueType.NameRef)
                        }))
                }
            );
        }
        
                
        [Test]
        public void DefaultTemplateOneValueUnderscore1()
        {
            Checker.CheckRule(
                "S -> \"123\" as value_test",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "123", localName: "value_test")
                        },
                        new Template(new []
                        {
                            new TemplateItem<(string, string)>("ValueTest", ("value_test", null), TemplateValueType.NameRef)
                        }))
                }
            );
        }
        
                        
        [Test]
        public void DefaultTemplateOneValueUnderscore2()
        {
            Checker.CheckRule(
                "S -> \"123\" as value_1",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "123", localName: "value_1")
                        },
                        new Template(new []
                        {
                            new TemplateItem<(string, string)>("Value1", ("value_1", null), TemplateValueType.NameRef)
                        }))
                }
            );
        }
        
        [Test]
        public void DefaultTemplateTwoValues()
        {
            Checker.CheckRule(
                "S -> \"123\" as test" + 
                     "\"222\" as number",
                new[]
                {
                    new Rule("S", 
                        new[]
                        {
                            new RuleItem(RuleItemType.Terminal, "123", localName: "test"),
                            new RuleItem(RuleItemType.Terminal, "222", localName: "number")
                        },
                        new Template(new []
                        {
                            new TemplateItem<(string, string)>("Test", ("test", null), TemplateValueType.NameRef),
                            new TemplateItem<(string, string)>("Number", ("number", null), TemplateValueType.NameRef)
                        }))
                }
            );
        }
        

    }
}