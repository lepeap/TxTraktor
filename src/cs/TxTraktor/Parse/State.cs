using System;
using System.Collections.Generic;
using TxTraktor.Compile.Model;
using TxTraktor.Parse.Forest;

namespace TxTraktor.Parse
{
    internal class State : IEquatable<State>
    {
        private readonly Rule _rule;
        private readonly int _startColumnIndex;
        private readonly List<State> _parentStates = new List<State>();
        private List<State> _childs;
        private  int _dotIndex;
        private int? _endColumnIndex;
        private string _createOpType;
        
        public State(Rule rule,
                     int dotIndex,
                     int startColumnIndex,
                     Node node,
                     string createOpType,
                     IEnumerable<State> parents = null,
                     State parent = null,
                     int? endColumnIndex=null
                     )
        {
            _rule = rule;
            _dotIndex = dotIndex;
            _startColumnIndex = startColumnIndex;
            _endColumnIndex = endColumnIndex;
            if (parent != null)
                _parentStates.Add(parent);
            if (parents!=null) 
                _parentStates.AddRange(parents);
            _createOpType = createOpType;
            Node = node;
        }

        public Node Node { get; } 
        
        public Rule Rule => _rule;
        
        public int DotIndex => _dotIndex;

        public int StartColumnIndex => _startColumnIndex;

        public int? EndColumnIndex
        {
            get =>_endColumnIndex;
        }

        public IEnumerable<State> Parents => _parentStates;

        public bool IsCompleted => DotIndex == Rule.TermsCount;

        public bool HasTemplate => Rule.HasTemplate;

        public bool IsValid => Node.IsValid;
        public bool IsSystemIntermediate => Rule.IsSystemIntermediate;

        public TermBase CurrentTerm => Rule[DotIndex];
        
        public void AddChild(State state)
        {
            if (_childs==null)
                _childs = new List<State>();
            _childs.Add(state);
        }
        
        public void AddParents(IEnumerable<State> states)
        {
            _parentStates.AddRange(states);
            if (_childs != null)
                foreach (var child in _childs)
                    child.AddParents(states);
            
        }

        public bool Equals(State other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_rule, other._rule) && _dotIndex == other._dotIndex && _startColumnIndex == other._startColumnIndex;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((State) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_rule != null ? _rule.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _dotIndex;
                hashCode = (hashCode * 397) ^ _startColumnIndex;
                return hashCode;
            }
        }

        public override string ToString()
        {
            var list = new List<string>();
            foreach (var term in _rule.Terms)
            {
                if (list.Count==_dotIndex)
                    list.Add("@");
                
                list.Add(term.ToString());
            }
            if (_rule.TermsCount==_dotIndex)
                list.Add("@");

            return $"{Rule.Name} '{string.Join(" ", list)}' start: {StartColumnIndex} end: {EndColumnIndex}";
        }
    }
}