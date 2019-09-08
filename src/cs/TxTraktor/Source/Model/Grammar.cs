using System.Collections.Generic;
using System.Linq;

namespace TxTraktor.Source.Model
{
    internal class Grammar
    {
        private readonly List<Rule> _rules = new List<Rule>();
        private readonly List<Import> _imports = new List<Import>();

        public Grammar()
        {
        }
        
        public Grammar(string name, 
                       IEnumerable<Rule> rules, 
                       IEnumerable<Import> imports = null,
                       string key = null,
                       Language lang = Language.Unknown
            )
        {
            
            _rules = rules.ToList();
            if (imports!=null)
                _imports = imports.ToList();
            Name = name;
            Key = key;
            Language = lang;
        }
        public string Key { get; set; }
        public string Name { get; set; }
        public Language Language { get; set; } = Language.Unknown;
        public bool HasErrors => Errors != null && Errors.Any();
        public string[] Errors { get; set; }
        public IEnumerable<Rule> Rules => _rules;
        public IEnumerable<Import> Imports => _imports;
        public bool HasGrammarImports => Imports != null && Imports.Any();

        public void AddRule(Rule rule)
        {
            _rules.Add(rule);
        }
        public void AddImport(Import import)
        {
            _imports.Add(import);
        }
    }
}