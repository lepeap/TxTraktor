using NUnit.Framework;
using TxTraktor;
using TxTraktor.Compile.Condition;
using TxTraktor.Tokenize;

namespace TxtTractor.Test.Compiler.Conditions
{
    [Parallelizable(ParallelScope.All)]
    public class StartText
    {
        [Test]
        public void StartTextTrue()
        {
            var token = new Token("11",0,5,7, new TextInfo(2, 7));
            Checker.CheckCondition<StartTextCondition>(token, true);
        }
        
        [Test]
        public void StartTextFalse()
        {
            var token = new Token("11",2,3,5, new TextInfo(3, 10));
            Checker.CheckCondition<StartTextCondition>(token, false);
        }
    }
}