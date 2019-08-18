using NUnit.Framework;
using TxTraktor.Compile.Condition;

namespace TxtTractor.Test.Compiler.Conditions
{
    [Parallelizable(ParallelScope.All)]
    public class StartsWithUpper
    {
        [Test]
        public void AllSmallCyrillic()
        {
            Checker.CheckCondition<StartsWithUpperCondition>("тест", false);
        }
        
        [Test]
        public void AllSmallLatin()
        {
            Checker.CheckCondition<StartsWithUpperCondition>("etalon", false);
        }
        
        [Test]
        public void AllSmallLatinAndCyrillic()
        {
            Checker.CheckCondition<StartsWithUpperCondition>("тестetalon", false);
        }
        
        [Test]
        public void StartsBigCyrillic()
        {
            Checker.CheckCondition<StartsWithUpperCondition>("Тест", true);
        }
                
        [Test]
        public void StartsBigLatin()
        {
            Checker.CheckCondition<StartsWithUpperCondition>("Test", true);
        }

        [Test]
        public void AllBigCyrillic()
        {
            Checker.CheckCondition<StartsWithUpperCondition>("ТЕСТ", true);
        }

        [Test] public void AllBigLatin()
        {
            Checker.CheckCondition<StartsWithUpperCondition>("TEST", true);
        }
    }
}