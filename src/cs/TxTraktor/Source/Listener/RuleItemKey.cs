using System;
using System.Linq;
using TxTraktor.Source.Model;

namespace TxTraktor.Source.Listener
{
    internal partial class Main
    {
        public bool InlineIsHead { get; set; }
        public override void EnterRule_item_static_main_key_full_nonterm(CfgGramParser.Rule_item_static_main_key_full_nontermContext context)
        {
            RuleItem.Key = context.GetText();
            RuleItem.Type = RuleItemType.NonTerminal;
        }
        
        public override void EnterRule_item_static_main_key_short_nonterm(CfgGramParser.Rule_item_static_main_key_short_nontermContext context)
        {
            RuleItem.Key = context.GetText();
            RuleItem.Type = RuleItemType.NonTerminal;
        }

        public override void EnterRule_item_static_main_key_lemma(CfgGramParser.Rule_item_static_main_key_lemmaContext context)
        {
            var text = context.GetText();
            RuleItem.Key = text.Substring(2, text.Length - 3);
            RuleItem.Type = RuleItemType.Lemma;
        }
        
        public override void EnterRule_item_static_main_key_reg(CfgGramParser.Rule_item_static_main_key_regContext context)
        {
            var text = context.GetText();
            RuleItem.Key = text.Substring(2, text.Length - 3);
            RuleItem.Type = RuleItemType.Regex;
        }
        
                
        public override void EnterRule_item_static_main_key_morph(CfgGramParser.Rule_item_static_main_key_morphContext context)
        {
            var text = context.GetText();
            RuleItem.Key = text.Substring(2, text.Length - 3);
            RuleItem.Type = RuleItemType.Morphology;
        }
        
        public override void EnterRule_item_static_main_key_string(CfgGramParser.Rule_item_static_main_key_stringContext context)
        {
            var text = context.GetText();
            RuleItem.Key = text.Substring(1, text.Length - 2);
            RuleItem.Type = RuleItemType.Terminal;
        }

        public override void EnterRule_item_variable_name(CfgGramParser.Rule_item_variable_nameContext context)
        {
            var text = context.GetText();
            RuleItem.Key = text.Substring(1, text.Length - 1);
            RuleItem.Type = RuleItemType.VariableName;
        }

        public override void EnterRule_inline_expression(CfgGramParser.Rule_inline_expressionContext context)
        {
            IsInline = true;
            InlineIsHead = RuleItem.IsHead;
        }

        public override void ExitRule_inline_expression(CfgGramParser.Rule_inline_expressionContext context)
        {
            IsInline = false;
            var rule = Grammar.Rules.Last();
            rule.Name  = $"{RulesStack.Peek().Name}:{Guid.NewGuid()}" ;
            rule.WasInline = true;
            RuleItem = new RuleItem();
            RuleItem.Type = RuleItemType.NonTerminal;
            RuleItem.Key = rule.Name;
            RuleItem.IsHead = InlineIsHead;
        }
    }
}