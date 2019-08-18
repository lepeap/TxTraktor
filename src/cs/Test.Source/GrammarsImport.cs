using NUnit.Framework;
using TxTraktor.Source.Model;

namespace TxtTractor.Test.Source
{
    [Parallelizable(ParallelScope.All)]
    public class GrammarsImport
    {
                
        [Test]
        public void GrammarNonTermDefault()
        {
            Checker.CheckGrammar(
                "grammar Test;\n"+
                "from Common import Dot;\n"+
                "S -> \"123\";"
                , new Grammar("Test",
                    new []
                    {
                        new Rule("S", new []
                        {
                            new RuleItem(RuleItemType.Terminal, "123") 
                        })
                    },
                    new []
                    {
                        new Import("Common", ImportType.NonTerminal, "Dot")
                    }
                ));
        }
        
        [Test]
        public void GrammarNonTerm()
        {
            Checker.CheckGrammar(
                "grammar Test;\n"+
                "from Common import Dot as DT;\n"+
                "S -> \"123\";"
                , new Grammar("Test",
                    new []
                    {
                        new Rule("S", new []
                        {
                            new RuleItem(RuleItemType.Terminal, "123") 
                        })
                    },
                    new []
                    {
                        new Import("Common", ImportType.NonTerminal, "Dot", "DT")
                    }
                ));
        }
        
        [Test]
        public void GrammarDefault()
        {
            Checker.CheckGrammar(
                "grammar Test;\n"+
                "import Common;\n"+
                "S -> \"123\";"
                , new Grammar("Test",
                    new []
                    {
                        new Rule("S", new []
                        {
                            new RuleItem(RuleItemType.Terminal, "123") 
                        })
                    },
                    new []
                    {
                        new Import("Common", ImportType.Grammar)
                    }
                ));
        }
        
        [Test]
        public void Grammar()
        {
            Checker.CheckGrammar(
                "grammar Test;\n"+
                "import Common as C;\n"+
                "S -> \"123\";"
                , new Grammar("Test",
                    new []
                    {
                        new Rule("S", new []
                        {
                            new RuleItem(RuleItemType.Terminal, "123") 
                        })
                    },
                    new []
                    {
                        new Import("Common", ImportType.Grammar, localName: "C")
                    }
                ));
        }

        [Test]
        public void GrammarMultiple()
        {
            Checker.CheckGrammar(
                "grammar Test;\n"+
                "from Testik import Test;\n"+
                "from Common import Dot as DT;\n"+
                "import Common as C;\n"+
                "import Common;\n"+
                "S -> \"123\";"
                , new Grammar("Test",
                    new []
                    {
                        new Rule("S", new []
                        {
                            new RuleItem(RuleItemType.Terminal, "123") 
                        })
                    },
                    new []
                    {
                        new Import("Testik",  ImportType.NonTerminal, "Test"),
                        new Import("Common",  ImportType.NonTerminal, "Dot", "DT"),
                        new Import("Common",  ImportType.Grammar, localName: "C"),
                        new Import("Common",  ImportType.Grammar)
                    }
                ));
        }
        
        
        
        
    }
}