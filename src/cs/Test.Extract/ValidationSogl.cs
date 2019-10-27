using NUnit.Framework;
using TxTraktor.Extract;

namespace Test.Extract
{
    [Parallelizable(ParallelScope.All)]
    public class ValidationSogl
    {
        [Test]
        public void SolgChisloTrue()
        {
            Checker.Check(
                "тест большой.",
                "S1 -> \"тест\";\n"+
                "S2 -> \"большой\";\n"+
                "S[Test=$name] -> S1<согл=1,число> S2<согл=1,число> #set name=\"1234\";",
                new []{
                    new ExtractionDic("Main.S", "тест большой", 0)
                    {
                        {"Test", new ExtractionValue( "1234", ValueType.String)}
                    }
                }
            );
        }
        
        [Test]
        public void SolgChisloFalse()
        {
            Checker.Check(
                "дом большие.",
                "S1 -> \"дом\";\n"+
                "S2 -> l\"большой\";\n"+
                "S[Test=$name] -> S1<согл=1,число> S2<согл=1,число> #set name=\"1234\";",
                new ExtractionDic[0]
            );
        }
    }
}