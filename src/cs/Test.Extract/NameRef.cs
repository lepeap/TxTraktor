using NUnit.Framework;
using TxTraktor.Extract;

namespace Test.Extract
{
    [Parallelizable(ParallelScope.All)]
    public class NameRef
    {
        [Test]
        public void RefToTerminal()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=$name] -> \"тест\" as name;",
                new []{
                    new ExtractionDic("Test.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue("тест", ValueType.String)}
                    }
                }
            );
        }
        
        [Test]
        public void RefToSecondTerminal()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=$name] -> \"тест\" \"1234\" as name;",
                new []{
                    new ExtractionDic("Test.S", "тест 1234", 0)
                    {
                        {"Test", new ExtractionValue("1234", ValueType.String)}
                    }
                }
            );
        }
        
        [Test]
        public void RefToNonTerminalWithoutTemplate()
        {
            Checker.Check(
                "тест 1234.",
                 "S1 -> \"тест\";"+
                       "S[Test=$name] -> S1 as name;",
                new []{
                    new ExtractionDic("Test.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue("тест", ValueType.String)}
                    }
                }
            );
        }
        
                
        [Test]
        public void RefToNonTerminalWithTemplate()
        {
            var embedDic = new ExtractionDic("Test.S1", "1234", 5)
            {
                {"Val", new ExtractionValue(123, ValueType.Int)}
            };
            Checker.Check(
                "тест 1234.",
                "S1[Val=123] -> \"1234\";"+
                "S[Test=$name] -> S1 as name \".\";",
                new []{
                    embedDic,
                    new ExtractionDic("Test.S", "1234.", 5)
                    {
                        {
                            "Test", 
                            new ExtractionValue(
                                embedDic, 
                                ValueType.Dictionary
                            )
                        }
                    }
                }
            );
        }
        
        [Test]
        public void RefToNonTerminalWithTemplateWithDefaultValue()
        {
            Checker.Check(
                "тест 1234.",
                "S1[Value=123] -> \"1234\";"+
                "S[Test=$name] -> S1 as name \".\";",
                new []{
                    new ExtractionDic("Test.S1", "1234", 5)
                    {
                        {"Value", new ExtractionValue(123, ValueType.Int)}
                    },
                    new ExtractionDic("Test.S", "1234.", 5)
                    {
                        {
                            "Test", 
                             new ExtractionValue(123, ValueType.Int)
                        }
                    }
                }
            );
        }
        
        [Test]
        public void RefToNonTerminalWithTemplateWithKey()
        {
            Checker.Check(
                "тест 1234.",
                "S1[Val=123] -> \"1234\";"+
                "S[Test=$name.Val] -> S1 as name \".\";",
                new []{
                    new ExtractionDic("Test.S1", "1234", 5)
                    {
                        {"Val", new ExtractionValue(123, ValueType.Int)}
                    },
                    new ExtractionDic("Test.S", "1234.", 5)
                    {
                        {
                            "Test", 
                            new ExtractionValue(
                                123, 
                                ValueType.Int
                            )
                        }
                    }
                }
            );
        }
        
        
        [Test]
        public void RefToTerminalWrongIndex()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=$name] -> \"тест\";",
                new []{
                    new ExtractionDic("Test.S", "тест", 0)
                    {
                        
                    }
                }
            );
        }
    }
}