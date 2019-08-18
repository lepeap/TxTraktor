using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using TxTraktor;
using TxTraktor.Compile.Condition;
using TxTraktor.Morphology;

namespace TxtTractor.Test.Compiler.Conditions
{
    [Parallelizable(ParallelScope.All)]
    public class Lemm
    {
        [Test]
        public void TestTrue()
        {
            var token = new Token("тест");
            token.Morphs = new[]
            {
                new MorphInfo("тест", new ReadOnlyDictionary<string, string>(new Dictionary<string, string>())), 
            };
            Checker.CheckCondition<LemmaCondition>(new []{"тест"}, token, true);
        }
        
        [Test]
        public void TestFalse()
        {
            var token = new Token("123");
            token.Morphs = new[]
            {
                new MorphInfo("123", new ReadOnlyDictionary<string, string>(new Dictionary<string, string>())), 
            };
            Checker.CheckCondition<LemmaCondition>(new []{"тест"}, token, false);
        }
    }
}