using System;
using TxTraktor.Source.Model.Extraction;

namespace TxTraktor.Source.Listener
{
    internal partial class Main
    {
        public string StaticVarName { get; set; }
        public TemplateValueType StaticVarType { get; set; }
        public string StaticVarValue { get; set; }

        public override void ExitRule_static_var(CfgGramParser.Rule_static_varContext context)
        {
            var rule = RulesStack.Peek();
            TemplateItemBase templateItem;
            switch (StaticVarType)
            {
                case TemplateValueType.String:
                    var value = StaticVarValue.Substring(1, StaticVarValue.Length - 2);
                    templateItem = new TemplateItem<string>(StaticVarName, value, StaticVarType);
                    break;
                case TemplateValueType.Bool:
                    templateItem = new TemplateItem<bool>(StaticVarName, StaticVarValue == "true", StaticVarType);
                    break;
                case TemplateValueType.Float:
                    templateItem = new TemplateItem<float>(StaticVarName, float.Parse(StaticVarValue), StaticVarType);
                    break;
                case TemplateValueType.Integer:
                    templateItem = new TemplateItem<int>(StaticVarName, int.Parse(StaticVarValue), StaticVarType);
                    break;
                default:
                    throw new NotImplementedException("Unknown static vat type");
            }
            
            rule.AddStaticVar(templateItem);
        }

        public override void EnterRule_static_var_name(CfgGramParser.Rule_static_var_nameContext context)
        {
            StaticVarName = context.GetText();
        }
        
        public override void ExitRule_static_var_value(CfgGramParser.Rule_static_var_valueContext context)
        {
            StaticVarValue = context.GetText();
        }

        public override void ExitRule_static_string_value(CfgGramParser.Rule_static_string_valueContext context)
        {
            StaticVarType = TemplateValueType.String;
        }
        
        public override void ExitRule_static_integer_value(CfgGramParser.Rule_static_integer_valueContext context)
        {
            StaticVarType = TemplateValueType.Integer;
        }
        public override void ExitRule_static_float_value(CfgGramParser.Rule_static_float_valueContext context)
        {
            StaticVarType = TemplateValueType.Float;
        }
        public override void ExitRule_static_bool_value(CfgGramParser.Rule_static_bool_valueContext context)
        {
            StaticVarType = TemplateValueType.Bool;
        }
    }
}