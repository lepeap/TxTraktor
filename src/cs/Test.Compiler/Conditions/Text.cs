using NUnit.Framework;
using TxTraktor;
using TxTraktor.Compile.Condition;

namespace TxtTractor.Test.Compiler.Conditions
{
    [Parallelizable(ParallelScope.All)]
    public class Text
    {
        [Test]
        public void TestCyrillicTrue()
        {
            Checker.CheckCondition<TextCondition>(new []{"тест"}, new Token("тест"), true);
        }
        
        [Test]
        public void TestLatinTrue()
        {
            Checker.CheckCondition<TextCondition>(new []{"test"}, new Token("test"), true);
        }
        
        [Test]
        public void TestNumberTrue()
        {
            Checker.CheckCondition<TextCondition>(new []{"123"}, new Token("123"), true);
        }
        
        [Test]
        public void TestFalse()
        {
            Checker.CheckCondition<TextCondition>(new []{"1234"}, new Token("123"), false);
        }
    }
}