using NUnit.Framework;
using TxTraktor.Extract;

namespace Test.Extract
{
    public class Simple
    {
        [Test]
        public void CoordinatesCheck()
        {
            Checker.Check(
                "[1234] тест",
                "RedactionAdderStart -> \"[\" as value;" +
                "RedactionAdderEnd -> \"]\" as value;" +
                "RedactionAdderCenter -> r\".+\" as first_word r\".+\"* as other_text;" +
                "RedactionAdder -> RedactionAdderStart as start_tag RedactionAdderCenter as center RedactionAdderEnd as end_tag;",
                new[]
                {
                    new ExtractionDic("Main.RedactionAdder", "[1234]", 0)
                    {
                        {
                            "StartTag", new ExtractionValue("[", ValueType.String)
                        },
                        {
                            "Center", new ExtractionValue(
                                new ExtractionDic("Main.RedactionAdderCenter", "1234", 1)
                                {
                                    {"FirstWord", new ExtractionValue("1234", ValueType.String)}
                                }, ValueType.Dictionary)
                        },
                        {
                            "EndTag", new ExtractionValue("]", ValueType.String)
                        },
                    }
                },
                null,
                null,
                null,
                "Main.RedactionAdder"
            );
        }
    }
}