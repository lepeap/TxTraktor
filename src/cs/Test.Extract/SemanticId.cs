using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TxTraktor.Extension;
using TxTraktor.Extract;

namespace TxtTractor.Test.Extract
{
    [Parallelizable(ParallelScope.All)]
    public class SemanticId
    {
        private IExtension _createExtensions(string extName, IEnumerable<Dictionary<string, string>> results)
        {
            var ext = Substitute.For<IExtension>();
            ext.Name.Returns(extName);
            ext.Process(Arg.Any<string>()).Returns(results);
            return ext;
        }
        
        [Test]
        public void OneRuleOneItem()
        {
            var extension = _createExtensions("test", new Dictionary<string, string>[]
            {
                new Dictionary<string, string>(){ 
                    {"test", "тест"},
                    {"test_id", "qwerty"}
                }
            });
            Checker.Check(
                "тест 1234.",
                "S[Test=$test] -> <test>test</end> $test;",
                new []{
                    new ExtractionDic("Main.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue( "тест", ValueType.String, "qwerty")}
                    }
                },
                extensions: new [] { extension }
            );
        }
    }
}