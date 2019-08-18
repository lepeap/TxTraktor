using TxTraktor.Source.Model;

namespace TxTraktor.Source.Listener
{
    internal partial class Main : CfgGramBaseListener
    {
        public override void EnterRule_item_star_counter(CfgGramParser.Rule_item_star_counterContext context)
        {
            RuleItem.Counter = Counter.Star;
        }
        
        public override void EnterRule_item_plus_counter(CfgGramParser.Rule_item_plus_counterContext context)
        {
            RuleItem.Counter = Counter.Plus;
        }
        
        public override void EnterRule_item_question_counter(CfgGramParser.Rule_item_question_counterContext context)
        {
            RuleItem.Counter = Counter.Question;
        }
        
        public override void EnterRule_item_number_counter(CfgGramParser.Rule_item_number_counterContext context)
        {
            var min = context.rule_item_number_counter_min().GetText();
            var max = int.Parse(context.rule_item_number_counter_max().GetText());
            CounterValue value;
            if (string.IsNullOrWhiteSpace(min))
                value = new CounterValue(0, max);
            else
                value = new CounterValue(int.Parse(min), max);
            
            RuleItem.Counter = Counter.Number;
            RuleItem.CounterValue = value;
        }
    }
}