using NUnit.Framework;
using TxTraktor.Extract;

namespace Test.Extract
{
    public class NonTerminalCondition
    {
        [Test]
        public void RegexOnNonTerminal()
        {
            Checker.Check(
                "123456 12345.",
                "S1 -> r\"1234.+\";\n"+
                "S[Test=$value] -> S1<regex=\"12345\\d+\"> as value;",
                new []{
                    new ExtractionDic("Main.S", "123456", 0)
                    {
                        {"Test", new ExtractionValue( "123456", ValueType.String)}
                    }
                }
            );
        }
        
        [Test]
        public void StartsBigOnNonTerminal()
        {
            Checker.Check(
                "Тест тест.",
                "S1 -> l\"тест\";\n"+
                "S[Test=$value] -> S1<нбол> as value;",
                new []{
                    new ExtractionDic("Main.S", "Тест", 0)
                    {
                        {"Test", new ExtractionValue( "Тест", ValueType.String)}
                    }
                }
            );
        }
    }
}