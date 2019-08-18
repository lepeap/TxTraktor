using System.Collections.Generic;
using System.Linq;
using TxTraktor.Source.Model.Extraction;

namespace TxTraktor.Source.Listener
{
    internal partial class Main
    {
        public Template Template { get; set; }
        
        public List<TemplateItemBase>  TemplateItems { get; set; }
        
        public string TemplateItemKey { get; set; }
        
        public override void EnterRule(CfgGramParser.RuleContext context)
        {
            Template = null;
        }
        
        public override void EnterRule_template(CfgGramParser.Rule_templateContext context)
        {
            Template = new Template();
        }

        public override void EnterRule_template_member_key(CfgGramParser.Rule_template_member_keyContext context)
        {
            TemplateItemKey = context.GetText();
            
        }
        
        public override void EnterRule_template_member_value(CfgGramParser.Rule_template_member_valueContext context)
        {
            TemplateItems = new List<TemplateItemBase>();
        }
        
        public override void ExitRule_template_member_value(CfgGramParser.Rule_template_member_valueContext context)
        {
            TemplateItemBase item;
            if (TemplateItems.Count == 1)
            {
                item = TemplateItems.First();
                item.Name = TemplateItemKey;
            }
            else
            {
                item = new TemplateItem<TemplateItemBase[]>(TemplateItemKey, 
                    TemplateItems.ToArray(),
                    TemplateValueType.List);

            }
            Template.AddItem(item);
        }

        public override void EnterRule_template_value_bool(CfgGramParser.Rule_template_value_boolContext context)
        {
            TemplateItems.Add(new TemplateItem<bool>()
            {
                Value = context.GetText()=="true", 
                Type = TemplateValueType.Bool
            });
        }
        
        public override void EnterRule_template_value_string(CfgGramParser.Rule_template_value_stringContext context)
        {var text = context.GetText();
            TemplateItems.Add(new TemplateItem<string>()
            {
                Value = text.Substring(1, text.Length - 2), 
                Type = TemplateValueType.String
            });
        }
        
        public override void EnterRule_template_value_integer(CfgGramParser.Rule_template_value_integerContext context)
        {
            TemplateItems.Add(new TemplateItem<int>()
            {
                Value = int.Parse(context.GetText()), 
                Type = TemplateValueType.Integer
            });
        }
        
        public override void EnterRule_template_value_float(CfgGramParser.Rule_template_value_floatContext context)
        {
            TemplateItems.Add(new TemplateItem<float>()
            {
                Value = float.Parse(context.GetText()), 
                Type = TemplateValueType.Float
            });
        }

        public override void EnterRule_template_value_name_reference(CfgGramParser.Rule_template_value_name_referenceContext context)
        {
            TemplateItems.Add(new TemplateItem<(string, string)>()
            {
                Value = (
                    context.rule_template_value_name_reference_key().GetText(), 
                    context.rule_template_value_name_reference_value()?.GetText()
                ), 
                Type = TemplateValueType.NameRef
            });
        }

        public override void EnterRule_template_value_number_reference(CfgGramParser.Rule_template_value_number_referenceContext context)
        {
            TemplateItems.Add(new TemplateItem<(int, string)>()
            {
                Value = (
                    int.Parse(context.rule_template_value_number_reference_key().GetText()), 
                    context.rule_template_value_number_reference_value()?.GetText()
                ), 
                Type = TemplateValueType.NumberRef
            });
        }
    }
}