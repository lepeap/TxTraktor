using NUnit.Framework;
using TxTraktor.Compile.Condition;

namespace TxtTractor.Test.Compiler.Conditions
{
    [Parallelizable(ParallelScope.All)]
    public class AllUppercase
    {
        [Test]
        public void AllSmallCyrillic()
        {
            Checker.CheckCondition<AllUppercaseCondition>("тест", false);
        }
        
        [Test]
        public void AllSmallLatin()
        {
            Checker.CheckCondition<AllUppercaseCondition>("etalon", false);
        }
        
        [Test]
        public void AllSmallLatinAndCyrillic()
        {
            Checker.CheckCondition<AllUppercaseCondition>("тестetalon", false);
        }
        
        [Test]
        public void StartsBigCyrillic()
        {
            Checker.CheckCondition<AllUppercaseCondition>("Тест", false);
        }
                
        [Test]
        public void StartsBigLatin()
        {
            Checker.CheckCondition<AllUppercaseCondition>("Test", false);
        }

        [Test]
        public void AllBigCyrillic()
        {
            Checker.CheckCondition<AllUppercaseCondition>("ТЕСТ", true);
        }

        [Test] public void AllBigLatin()
        {
            Checker.CheckCondition<AllUppercaseCondition>("TEST", true);
        }
    }
}