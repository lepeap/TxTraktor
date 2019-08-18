using NUnit.Framework;
using TxTraktor;
using TxTraktor.Compile.Condition;

namespace TxtTractor.Test.Compiler.Conditions
{
    [Parallelizable(ParallelScope.All)]
    public class Reg
    {
        [Test]
        public void TestTrue1()
        {
            Checker.CheckCondition<RegexCondition>(new []{"тест"}, new Token("тест"), true);
        }
        
        [Test]
        public void TestTrue2()
        {
            Checker.CheckCondition<RegexCondition>(new []{"тест.*"}, new Token("тест123"), true);
        }
        
        [Test]
        public void TestTrue3()
        {
            Checker.CheckCondition<RegexCondition>(new []{"тест1?"}, new Token("тест"), true);
        }
        
        [Test]
        public void TestFalse1()
        {
            Checker.CheckCondition<RegexCondition>(new []{"тест"}, new Token("123"), false);
        }
    }
}