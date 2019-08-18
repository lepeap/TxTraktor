using NUnit.Framework;
using TxTraktor.Compile.Condition;

namespace TxtTractor.Test.Compiler.Conditions
{
    [Parallelizable(ParallelScope.All)]
    public class AllLowercase
    {
        [Test]
        public void AllSmallCyrillic()
        {
            Checker.CheckCondition<AllLowercaseCondition>("тест", true);
        }
        
        [Test]
        public void AllSmallLatin()
        {
            Checker.CheckCondition<AllLowercaseCondition>("etalon", true);
        }
        
        [Test]
        public void AllSmallLatinAndCyrillic()
        {
            Checker.CheckCondition<AllLowercaseCondition>("тестetalon", true);
        }
        
        [Test]
        public void StartsBigCyrillic()
        {
            Checker.CheckCondition<AllLowercaseCondition>("Тест", false);
        }
                
        [Test]
        public void StartsBigLatin()
        {
            Checker.CheckCondition<AllLowercaseCondition>("Test", false);
        }

        [Test]
        public void AllBigCyrillic()
        {
            Checker.CheckCondition<AllLowercaseCondition>("ТЕСТ", false);
        }

        [Test] public void AllBigLatin()
        {
            Checker.CheckCondition<AllLowercaseCondition>("TEST", false);
        }
    }
}