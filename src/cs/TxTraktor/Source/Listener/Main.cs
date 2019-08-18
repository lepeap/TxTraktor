using System.Collections.Generic;
using TxTraktor.Source.Model;

namespace TxTraktor.Source.Listener
{
    internal partial class Main : CfgGramBaseListener
    {
        public Grammar Grammar { get; set; }
        public Stack<Rule> RulesStack { get; set; } = new Stack<Rule>();
        
        
        public override void EnterGram(CfgGramParser.GramContext context)
        {
            Grammar = new Grammar();
        }

        public override void EnterGrammar_name(CfgGramParser.Grammar_nameContext context)
        {
            Grammar.Name = context.GetText();
        }

        public override void EnterGrammar_lang(CfgGramParser.Grammar_langContext context)
        {
            Grammar.Language = LanguageExt.GetEnumFromKey(context.GetText());
        }
    }
}