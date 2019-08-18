using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using TxTraktor;
using TxTraktor.Compile;
using TxTraktor.Compile.Condition;
using TxTraktor.Morphology;
using TxTraktor.Source.Model;
using TxTraktor.Tokenize;

namespace TxtTractor.Test.Compiler.Conditions
{
    [Parallelizable(ParallelScope.All)]
    public class Manager
    {
        private ConditionManager _manager = new ConditionManager();

        private void check(RuleItem item, Dictionary<Token, bool> tasks)
        {
            var rezCond = _manager.GetCondition(item);
            foreach (var kp in tasks)
            {
                var rez = rezCond.IsValid(kp.Key);
                Assert.AreEqual(kp.Value,
                    rez,
                    "Wrong result for condition '{1}' for text '{0}'",
                    rezCond,
                    kp.Key);
            }
        }
        
        [Test]
        public void MainText()
        {
            var item = new RuleItem();
            item.Type = RuleItemType.Terminal;
            item.Key = "123";
            check(item, new Dictionary<Token, bool>()
            {
                {new Token("123"), true},
                {new Token("1234"), false}
            });
        }

        [Test]
        public void MainRegex()
        {
            var item = new RuleItem();
            item.Type = RuleItemType.Regex;
            item.Key = @"123\d*";
            check(item, new Dictionary<Token, bool>()
            {
                {new Token("123"), true},
                {new Token("1234"), true},
                {new Token("123t"), false}
            });
        }
        
        [Test]
        public void MainLemma()
        {
            var item = new RuleItem();
            item.Type = RuleItemType.Lemma;
            item.Key = "тест";
            check(item, new Dictionary<Token, bool>()
            {
                {new Token("тест"), true},
                {new Token("тестов")
                {
                    Morphs = new []
                    {
                        new MorphInfo("тест", 
                            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>())),
                    }
                }, true},
                {new Token("мост"), false}
            });
        }

        [Test]
        public void MainMorph()
        {
            var item = new RuleItem();
            item.Type = RuleItemType.Morphology;
            item.Key = "ед";
            check(item, new Dictionary<Token, bool>()
            {
                {new Token("тест"){
                    Morphs = new []
                    {
                        new MorphInfo("тест", 
                            new ReadOnlyDictionary<string, string>(
                                new Dictionary<string, string>(){{"число", "ед"}})),
                    }
                }, true},
                {new Token("тесты")
                {
                    Morphs = new []
                    {
                        new MorphInfo("тест", 
                            new ReadOnlyDictionary<string, string>(
                                new Dictionary<string, string>(){{"число", "мн"}})),
                    }
                }, false},
                {new Token("мост"), false}
            });
        }
        
        [Test]
        public void StartText()
        {
            foreach (var condKey in new[]{"начало", "start"})
            {
                var item = new RuleItem();
                item.Type = RuleItemType.Terminal;
                item.Key = "123";
                item.AddCondition(new Condition(condKey, false));
                check(item, new Dictionary<Token, bool>()
                {
                    {new Token("123", 0, 0, 3, null), true},
                    {new Token("123", 1, 2, 3, null), false}
                });
            }
        }
        
        [Test]
        public void EndText()
        {
            foreach (var condKey in new[]{"конец", "end"})
            {
                var item = new RuleItem();
                item.Type = RuleItemType.Terminal;
                item.Key = "123";
                item.AddCondition(new Condition(condKey, false));
                check(item, new Dictionary<Token, bool>()
                {
                    {new Token("123", 2, 5, 8, new TextInfo(3, 8)), true},
                    {new Token("123", 1, 2, 3, new TextInfo(5, 8)), false}
                });
            }
        }
        
        [Test]
        public void AllUpper()
        {
            foreach (var condKey in new[]{"вбол", "abig"})
            {
                var item = new RuleItem();
                item.Type = RuleItemType.Regex;
                item.Key = ".*";
                item.AddCondition(new Condition(condKey, false));
                check(item, new Dictionary<Token, bool>()
                {
                    {new Token("ТЕСТ"), true},
                    {new Token("Тест"), false},
                    {new Token("тест"), false}
                });
            }
        }
        
        [Test]
        public void AllLower()
        {
            foreach (var condKey in new[]{"вмал", "asmall"})
            {
                var item = new RuleItem();
                item.Type = RuleItemType.Regex;
                item.Key = ".*";
                item.AddCondition(new Condition(condKey, false));
                check(item, new Dictionary<Token, bool>()
                {
                    {new Token("ТЕСТ"), false},
                    {new Token("Тест"), false},
                    {new Token("тест"), true}
                });
            }
        }
        
        [Test]
        public void StartsUpper()
        {
            foreach (var condKey in new[]{"нбол", "sbig"})
            {
                var item = new RuleItem();
                item.Type = RuleItemType.Regex;
                item.Key = ".*";
                item.AddCondition(new Condition(condKey, false));
                check(item, new Dictionary<Token, bool>()
                {
                    {new Token("ТЕСТ"), true},
                    {new Token("Тест"), true},
                    {new Token("тест"), false}
                });
            }
        }
        
        [Test]
        public void WrongConditionArgArray()
        {
            Assert.Throws<WrongConditionArgsException>(() =>
            {
                var item = new RuleItem();
                item.Type = RuleItemType.Terminal;
                item.Key = "123";
                item.AddCondition(new Condition("морф", false));
                var rezCond = _manager.GetCondition(item);
            });
        }
        
        [Test]
        public void WrongConditionArgString()
        {
            Assert.Throws<WrongConditionArgsException>(() =>
            {
                var item = new RuleItem();
                item.Type = RuleItemType.Terminal;
                item.Key = "123";
                item.AddCondition(new Condition("рег", false));
                var rezCond = _manager.GetCondition(item);
            });
            
            Assert.Throws<WrongConditionArgsException>(() =>
            {
                var item = new RuleItem();
                item.Type = RuleItemType.Terminal;
                item.Key = "123";
                item.AddCondition(new Condition("рег", new []{"123","123"}));
                var rezCond = _manager.GetCondition(item);
            });
        }
    }
}