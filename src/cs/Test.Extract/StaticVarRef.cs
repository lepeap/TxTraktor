using NUnit.Framework;
using TxTraktor.Extract;

namespace TxtTractor.Test.Extract
{
    [Parallelizable(ParallelScope.All)]
    public class StaticVarRef
    {
        [Test]
        public void OneTerminalStaticVarString()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=$name] -> \"тест\" #set name=\"1234\";",
                new []{
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue( "1234", ValueType.String)}
                    }
                }
            );
        }
        
        [Test]
        public void OneTerminalStaticVarInt()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=$name] -> \"тест\" #set name=1234;",
                new []{
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue( 1234, ValueType.Int)}
                    }
                }
            );
        }
        
        [Test]
        public void OneTerminalStaticVarFloat()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=$name] -> \"тест\" #set name=0.1234;",
                new []{
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue( (float)0.1234, ValueType.Float)}
                    }
                }
            );
        }

        [Test]
        public void OneTerminalStaticVarBool()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=$name] -> \"тест\" #set name=true;",
                new []{
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue( true, ValueType.Bool)}
                    }
                }
            );
        }
        
                        
        [Test]
        public void OneTerminalStaticMultipleTypes()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test=$name, RealTest=$test] -> \"тест\" #set name=true #set test=123;",
                new []{
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue( true, ValueType.Bool)},
                        {"RealTest", new ExtractionValue( 123, ValueType.Int)}
                    }
                }
            );
        }
    }
}