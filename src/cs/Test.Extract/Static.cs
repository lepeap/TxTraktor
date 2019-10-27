using NUnit.Framework;
using TxTraktor.Extract;

namespace Test.Extract
{
    [Parallelizable(ParallelScope.All)]
    public class Static
    {
        [Test]
        public void OneTerminalStaticInt()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=1] -> \"тест\";",
                new[]
                {
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue(1, ValueType.Int)}
                    }
                }
            );
        }

        [Test]
        public void TwoTerminalStaticInt()
        {
            Checker.Check(
                "Тест тест 1234 аваав .",
                "S[Test=1] -> \"тест\" \"1234\";",
                new[]
                {
                    new ExtractionDic("Main.S", "тест 1234", 5)
                    {
                        {"Test", new ExtractionValue(1, ValueType.Int)}
                    }
                }
            );
        }

        [Test]
        public void OneTerminalStaticFloat()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=1.1] -> \"тест\";",
                new[]
                {
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue((float) 1.1, ValueType.Float)}
                    }
                }
            );
        }

        [Test]
        public void OneTerminalStaticString()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=\"хер\"] -> \"тест\";",
                new[]
                {
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue("хер", ValueType.String)}
                    }
                }
            );
        }

        [Test]
        public void OneTerminalStaticBool()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=true] -> \"тест\";",
                new[]
                {
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue(true, ValueType.Bool)}
                    }
                }
            );
        }


        [Test]
        public void AdditionalRulesToExtract()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=true] -> \"тест\";" +
                "S1[Test=true] -> \"1234\";",
                new[]
                {
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue(true, ValueType.Bool)}
                    }
                },
                null,
                null,
                "Main.S"
            );
        }
    }
}