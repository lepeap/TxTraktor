namespace TxTraktor.Source.Model.Extraction
{
    internal abstract class TemplateItemBase
    {
        public TemplateItemBase()
        {
        }
        public TemplateItemBase(string name, TemplateValueType type)
        {
            Name = name;
            Type = type;
        }
        public string Name { get;  set; }
        public TemplateValueType Type { get;  set; }


        public T As<T>() where T : TemplateItemBase
        {
            return (T) this;
        }

        public abstract TemplateItemBase Copy();

    }
}