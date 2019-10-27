using NUnit.Framework;
using TxTraktor.Extract;

namespace Test.Extract
{
    [Parallelizable(ParallelScope.All)]
    public class NumberRef
    {
        [Test]
        public void RefToTerminal()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=$0] -> \"тест\";",
                new []{
                    new ExtractionDic("Main.S", "тест", 0)
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
                "S[Test=$1] -> \"тест\" \"1234\";",
                new []{
                    new ExtractionDic("Main.S", "тест 1234", 0)
                    {
                        {"Test", new ExtractionValue("1234", ValueType.String)}
                    }
                }
            );
        }
        
        [Test]
        public void RefToTerminalWrongIndex()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=$1] -> \"тест\";",
                new []{
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        
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
                       "S[Test=$0] -> S1;",
                new []{
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue("тест", ValueType.String)}
                    }
                }
            );
        }
        
                
        [Test]
        public void RefToNonTerminalWithTemplate()
        {
            var embedDic = new ExtractionDic("Main.S1", "1234", 5)
            {
                {"Val", new ExtractionValue(123, ValueType.Int)}
            };
            Checker.Check(
                "тест 1234.",
                "S1[Val=123] -> \"1234\";"+
                "S[Test=$0] -> S1 \".\";",
                new []{
                    embedDic,
                    new ExtractionDic("Main.S", "1234.", 5)
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
                "S[Test=$0] -> S1 \".\";",
                new []{
                    new ExtractionDic("Main.S1", "1234", 5)
                    {
                        {"Value", new ExtractionValue(123, ValueType.Int)}
                    },
                    new ExtractionDic("Main.S", "1234.", 5)
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
                "S[Test=$0.Val] -> S1 \".\";",
                new []{
                    new ExtractionDic("Main.S1", "1234", 5)
                    {
                        {"Val", new ExtractionValue(123, ValueType.Int)}
                    },
                    new ExtractionDic("Main.S", "1234.", 5)
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
    }
}