using System.Collections.Generic;
using System.Linq;

namespace TxTraktor.Source.Model.Extraction
{
    internal class Template
    {
        private readonly List<TemplateItemBase> _items = new List<TemplateItemBase>();
        
        public Template()
        {
        }
        public Template(IEnumerable<TemplateItemBase> items, string name = null)
        {
            if (items != null)
                _items = items.ToList();
            Name = name;
        }
        
        public string Name { get; set; }
        
        public IEnumerable<TemplateItemBase> Items => _items;

        public int ItemsCount => Items.Count();

        public TemplateItemBase this[int i] => _items[i];


        public void AddItem(TemplateItemBase item)
        {
            _items.Add(item);
        }

        public Template Copy()
        {
            var newItems = Items.Select(item => item.Copy());
            return new Template(newItems, Name);
        }

    }
}