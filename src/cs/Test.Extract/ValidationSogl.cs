using System.Collections.Generic;
using NUnit.Framework;
using TxTraktor.Extract;

namespace TxtTractor.Test.Extract
{
    [Parallelizable(ParallelScope.All)]
    public class ValidationSogl
    {
        [Test]
        public void SolgChisloTrue()
        {
            var mb = new MorphMoqBuilder();
            mb.AddMorphData("тест", new Dictionary<string, string>()
            {
                {"число", "ед"}
            });
            mb.AddMorphData("большой", new Dictionary<string, string>()
            {
                {"число", "ед"}
            });

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
                },
                morph: mb.Result
            );
        }
        
        [Test]
        public void SolgChisloFalse()
        {
            var mb = new MorphMoqBuilder();
            mb.AddMorphData("дом", new Dictionary<string, string>()
            {
                {"число", "ед"}
            });
            mb.AddMorphData("большие", new Dictionary<string, string>()
            {
                {"число", "мн"}
            }, "большой");
            
            Checker.Check(
                "дом большие.",
                "S1 -> \"дом\";\n"+
                "S2 -> l\"большой\";\n"+
                "S[Test=$name] -> S1<согл=1,число> S2<согл=1,число> #set name=\"1234\";",
                new ExtractionDic[0],
                morph: mb.Result
            );
        }
    }
}