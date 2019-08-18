using System.Text;
using TxTraktor.Compile.Condition;

namespace TxTraktor.Compile.Model
{
    internal abstract class TermBase
    {
        private string _id;
        protected string _Name;
        public TermBase(
            bool isTerminal,
            bool isNullable = false,
            string localName = null,
            ICondition condition = null,
            bool isHead = false,
            string name = null,
            string semanticId = null)
        {
            IsTerminal = isTerminal;
            IsNullable = isNullable;
            LocalName = localName;
            Condition = condition;
            IsHead = isHead;
            SemanticId = semanticId;
            _Name = name;
        }

        public string Id
        {
            get
            {
                if (_id == null)
                {
                    var sb = new StringBuilder();
            
                    if (IsHead)
                        sb.Append('^');

                    if (_Name != null)
                        sb.Append(_Name);

                    if (Condition != null)
                        sb.Append(Condition);

                    if (LocalName != null)
                    {
                        sb.Append("as ");
                        sb.Append(LocalName);
                    }

                    _id = sb.ToString();
                }

                return _id;
            }
        }
        
        public bool IsTerminal { get; }
        public bool IsNullable { get; }
        public string LocalName { get; }
        public ICondition Condition { get; }
        public bool IsHead { get; }
        
        public string SemanticId { get; }

        public string ConditionKey => Condition?.ToString();


        public bool HasLocalName => !string.IsNullOrEmpty(LocalName);
        
        public bool HasCondition => Condition!=null;
        public bool IsValid(Token token)
        {
            if (Condition == null) return true;

            return Condition.IsValid(token);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}