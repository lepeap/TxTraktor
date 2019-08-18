using NUnit.Framework;
using TxTraktor;
using TxTraktor.Compile.Condition;
using TxTraktor.Tokenize;

namespace TxtTractor.Test.Compiler.Conditions
{
    [Parallelizable(ParallelScope.All)]
    public class EndText
    {
        [Test]
        public void EndTextTrue()
        {
            var token = new Token("11",1,5,7, new TextInfo(2, 7));
            Checker.CheckCondition<EndTextCondition>(token, true);
        }
        
        [Test]
        public void EndTextFalse()
        {
            var token = new Token("11",0,0,2, new TextInfo(3, 10));
            Checker.CheckCondition<EndTextCondition>(token, false);
        }
    }
}