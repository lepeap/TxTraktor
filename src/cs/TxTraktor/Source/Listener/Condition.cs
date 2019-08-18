using System.Collections.Generic;
using System.Linq;
using TxTraktor.Source.Model;

namespace TxTraktor.Source.Listener
{
    internal partial class Main
    {
        public Condition Condition { get; set; }
        
        public List<string> ConditionValues { get; set; } 
        public override void EnterRule_item_condition(CfgGramParser.Rule_item_conditionContext context)
        {
            Condition = new Condition();
            ConditionValues = new List<string>();
        }

        public override void ExitRule_item_condition(CfgGramParser.Rule_item_conditionContext context)
        {
            Condition.Values = ConditionValues.Any() ? ConditionValues.ToArray() : null;
            RuleItem.AddCondition(Condition);
        }

        public override void EnterRule_item_condition_negation(CfgGramParser.Rule_item_condition_negationContext context)
        {
            Condition.Negation = true;
        }

        public override void EnterRule_item_condition_flag(CfgGramParser.Rule_item_condition_flagContext context)
        {
            Condition.Key = context.GetText();
        }

        public override void EnterRule_item_condition_key(CfgGramParser.Rule_item_condition_keyContext context)
        {
            Condition.Key = context.GetText();
        }

        public override void EnterRule_item_condition_value_literal(CfgGramParser.Rule_item_condition_value_literalContext context)
        {
            ConditionValues.Add(context.GetText());
        }

        public override void EnterRule_item_condition_value_string(CfgGramParser.Rule_item_condition_value_stringContext context)
        {
            var text = context.GetText();
            ConditionValues.Add(text.Substring(1, text.Length - 2));
        }
    }
}