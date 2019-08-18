using System.Collections.Generic;
using System.Linq;
using TxTraktor.Source.Model.Extraction;

namespace TxTraktor.Source.Model
{
    internal class Rule
    {
        private readonly List<TemplateItemBase> _staticVars = new List<TemplateItemBase>();
        private List<RuleItem> _items = new List<RuleItem>();
        
        public Rule()
        {
            
        }
        
        public Rule(string name,
            IEnumerable<RuleItem> items,
            Template template = null,
            IEnumerable<TemplateItemBase> staticVars = null,
            bool isPossibleList = false,
            string extensionType = null,
            string extensionQuery = null
            )
        {
            _items = items.ToList();
            if (staticVars!=null)
                _staticVars = staticVars.ToList();

            Name = name;
            Template = template;
            IsPossibleList = isPossibleList;
            ExtensionType = extensionType;
            ExtensionQuery = extensionQuery;
        }
        public string Name { get; set; }
        public IEnumerable<RuleItem> Items => _items;
        public Template Template { get; set; }
        public bool IsPossibleList { get; set; }
        public bool WasInline { get; set; }
        public string ExtensionType { get; set; }
        public string ExtensionQuery { get; set; }
        public IEnumerable<TemplateItemBase> StaticVars => _staticVars;
        public bool HasStaticVars => StaticVars != null && StaticVars.Any();
        public bool HasTemplate => Template != null;
        public int TermsCount => Items.Count();
        public bool HasTerms => Items.Any();

        public void AddItem(RuleItem item)
        {
            _items.Add(item);
        }
        public void AddItemsRange(IEnumerable<RuleItem> items)
        {
            _items.AddRange(items);
        }
        public void SetNewItems(IEnumerable<RuleItem> items)
        {
            _items = items.ToList();
        }
        public void AddStaticVar(TemplateItemBase item)
        {
            _staticVars.Add(item);
        }

        public Rule Copy()
        {
            var newItems = Items.Select(item => item.Copy());
            var template = Template?.Copy();
            return new Rule(Name, newItems, template, StaticVars, IsPossibleList, ExtensionType, ExtensionQuery);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}