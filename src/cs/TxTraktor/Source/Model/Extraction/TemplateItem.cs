

namespace TxTraktor.Source.Model.Extraction
{
    internal class TemplateItem<T> : TemplateItemBase
    {
        public TemplateItem()
        {
            
        }
        public TemplateItem(string name, T value, TemplateValueType type) 
               : base(name, type)
        {
            Value = value;
        }
        public T Value { get; set; }
        public override TemplateItemBase Copy()
        {
            return new TemplateItem<T>(Name, Value, Type);
        }
    }
}