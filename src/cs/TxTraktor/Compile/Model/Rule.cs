using System.Collections.Generic;
using System.Linq;
using TxTraktor.Compile.Validation;
using TxTraktor.Source.Model.Extraction;

namespace TxTraktor.Compile.Model
{
    internal class Rule
    {
        private readonly TermBase[] _terms;
        private readonly Template _template;

        public Rule(string name, 
                    IEnumerable<TermBase> terms,
                    bool isStart = false,
                    Template template = null,
                    TemplateItemBase[] staticVars= null,
                    bool isPossibleList = false,
                    IValidator validator = null)
        {
            _terms = terms.ToArray();
            _template = template;
            Name = name;
            IsStart = isStart;
            StaticVars = staticVars;
            IsPossibleList = isPossibleList;
            Validator = validator;
        }
        public string Name { get; }
        public bool IsStart { get; }
        public bool IsPossibleList { get; }
        public TemplateItemBase[] StaticVars { get; }
        public IValidator Validator { get; }
        public IEnumerable<TermBase> Terms => _terms;
        public Template Template => _template;
        public bool HasStaticVars => StaticVars != null && StaticVars.Length > 0;
        public bool HasTemplate => _template != null;
        public bool HasTerms => Terms != null && Terms.Any();
        public int TermsCount => HasTerms ? Terms.Count() : 0;
        public bool HasValidator => Validator != null;
        public TermBase this[int index] => _terms[index];

        public override string ToString()
        {
            return Name;
        }
    }
}