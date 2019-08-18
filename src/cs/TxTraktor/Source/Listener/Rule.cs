using System.Collections.Generic;
using System.Linq;
using TxTraktor.Source.Model;
using TxTraktor.Source.Model.Extraction;

namespace TxTraktor.Source.Listener
{
    internal partial class Main
    {
        public bool IsInline { get; set; }
        public RuleItem RuleItem { get; set; }
        public string RuleName { get; set; }
        public string ExtensionType { get; set; }
        public string ExtensionQuery { get; set; }
        public List<Rule> CurrentRulesList { get; set; }
        
        public override void EnterRules(CfgGramParser.RulesContext context)
        {
            RulesStack.Clear();
        }
        

        public override void EnterRule_expression(CfgGramParser.Rule_expressionContext context)
        {
            var rule = new Rule();
            rule.Name = RuleName;
            RulesStack.Push(rule);
        }
        public override void ExitRule_expression(CfgGramParser.Rule_expressionContext context)
        {
            var rule = RulesStack.Pop();
            rule.Template = Template;
            rule.ExtensionType = ExtensionType;
            rule.ExtensionQuery = ExtensionQuery;
            
            if (rule.Template == null && rule.Items.Any(x => x.HasLocalName))
            {
                var templItems = new List<TemplateItemBase>();
                foreach (var item in rule.Items.Where(x=>x.HasLocalName))
                {
                    var propMame = string.Join(
                        string.Empty,
                        item.LocalName
                            .Split('_')
                            .Select(x => x.First().ToString().ToUpper() + x.Substring(1)
                            )
                    );
                    templItems.Add(new TemplateItem<(string, string)>(propMame, (item.LocalName, null), TemplateValueType.NameRef));

                }
                rule.Template = new Template(templItems);
            }

            ExtensionType = null;
            ExtensionQuery = null;
            Grammar.AddRule(rule);
        }
        

        public override void EnterRule_expression_list(CfgGramParser.Rule_expression_listContext context)
        {
            CurrentRulesList = new List<Rule>();
        }

        public override void EnterRule_name(CfgGramParser.Rule_nameContext context)
        {
            RuleName = context.GetText();
        }

        public override void EnterRule_extension_query(CfgGramParser.Rule_extension_queryContext context)
        {
            var type = context.rule_extention_type().GetText();
            var query = context.rule_extention_query_text().GetText();
            // start tag + </end> length
            query = query.Substring(1, query.Length - 7 );
            ExtensionQuery = query;
            ExtensionType = type;
        }

        public override void EnterRule_item(CfgGramParser.Rule_itemContext context)
        {
            RuleItem = new RuleItem();
        }

        public override void ExitRule_item(CfgGramParser.Rule_itemContext context)
        {
            RulesStack.Peek().AddItem(RuleItem);
        }

        public override void EnterRule_item_head_flag(CfgGramParser.Rule_item_head_flagContext context)
        {
            RuleItem.IsHead = true;
        }
        
        public override void EnterRule_item_local_name(CfgGramParser.Rule_item_local_nameContext context)
        {
            RuleItem.LocalName = context.GetText();
        }
    }
}