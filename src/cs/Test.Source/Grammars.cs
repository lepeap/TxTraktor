using NUnit.Framework;
using TxTraktor;
using TxTraktor.Source.Model;

namespace TxtTractor.Test.Source
{
    [Parallelizable(ParallelScope.All)]
    public class Grammars
    {
        [Test]
        public void Simple()
        {
            Checker.CheckGrammar(
                "grammar Test;\n"+
                "S -> \"123\";"
                , new Grammar("Test",
                     new []
                     {
                         new Rule("S", new []
                         {
                             new RuleItem(RuleItemType.Terminal, "123") 
                         })
                     }
            ));
        }
        
        [Test]
        public void RuLanguage()
        {
            Checker.CheckGrammar(
                "grammar Test;lang ru;\n"+
                "S -> \"123\";"
                , new Grammar("Test",
                    new []
                    {
                        new Rule("S", new []
                        {
                            new RuleItem(RuleItemType.Terminal, "123") 
                        })
                    },
                    lang: Language.Ru
                ));
        }

        [Test]
        public void UnknownLanguage()
        {
            Checker.CheckGrammar(
                "grammar Test;lang en;\n"+
                "S -> \"123\";"
                , new Grammar("Test",
                    new []
                    {
                        new Rule("S", new []
                        {
                            new RuleItem(RuleItemType.Terminal, "123") 
                        })
                    }
                ));
        }
    }
}