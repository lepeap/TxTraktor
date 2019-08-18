using NUnit.Framework;
using TxTraktor;
using TxTraktor.Compile.Condition;

namespace TxtTractor.Test.Compiler.Conditions
{
    internal static class Checker
    {
        public static void CheckCondition<T>(string[] args, Token token, bool etalonResult) where T : ICondition, new()
        {
            var cond = new T();
            cond.Init(args);
            var result = cond.IsValid(token);
            Assert.AreEqual(etalonResult,
                result,
                "Wrong result item for condition type '{0}' and token '{1}'",
                typeof(T),
                token);
        }
        
        public static void CheckCondition<T>(Token token, bool etalonResult) where T : ICondition, new()
        {
            CheckCondition<T>(new string[0], token, etalonResult);
        }
        
        public static void CheckCondition<T>(string text, bool etalonResult) where T : ICondition, new()
        {
            CheckCondition<T>(new string[0], new Token(text), etalonResult);
        }
    }
}