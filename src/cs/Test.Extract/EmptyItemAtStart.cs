using NUnit.Framework;
using TxTraktor.Extract;

namespace Test.Extract
{
    public class EmptyItemAtStart
    {
        [Test]
        public void OneQuestionAtStart()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=true] -> \"fff\"? \"1234\";",
                new[]
                {
                    new ExtractionDic("Main.S", "1234", 5)
                    {
                        {"Test", new ExtractionValue(true, ValueType.Bool)}
                    }
                },
                null,
                null,
                null,
                "Main.S"
            );
        }
        
        [Test]
        public void TwoQuestionsAtStart()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=true] -> \"fff\"? \"123\"? \"1234\";",
                new[]
                {
                    new ExtractionDic("Main.S", "1234", 5)
                    {
                        {"Test", new ExtractionValue(true, ValueType.Bool)}
                    }
                },
                null,
                null,
                null,
                "Main.S"
            );
        }
        
        [Test]
        public void FiveQuestionsAtStart()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=true] -> \"fff\"? \"123\"? \"fff1\"?  \"fff1\"?  \"fff1\"? \"1234\";",
                new[]
                {
                    new ExtractionDic("Main.S", "1234", 5)
                    {
                        {"Test", new ExtractionValue(true, ValueType.Bool)}
                    }
                },
                null,
                null,
                null,
                "Main.S"
            );
        }
        
        [Test]
        public void OneStarAtStart()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=true] -> \"fff\"* \"1234\";",
                new[]
                {
                    new ExtractionDic("Main.S", "1234", 5)
                    {
                        {"Test", new ExtractionValue(true, ValueType.Bool)}
                    }
                },
                null,
                null,
                null,
                "Main.S"
            );
        }
        
                
        [Test]
        public void TwoStarAtStart()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=true] -> \"fff\"* \"fff\"* \"1234\";",
                new[]
                {
                    new ExtractionDic("Main.S", "1234", 5)
                    {
                        {"Test", new ExtractionValue(true, ValueType.Bool)}
                    }
                },
                null,
                null,
                null,
                "Main.S"
            );
        }
    }
}