using System.Collections.Generic;
using System.Linq;

namespace TxTraktor.Source.Model
{
    internal class RuleItem
    {
        private readonly List<Condition> _conditions = new List<Condition>();
        public RuleItem()
        {
            
        }
        
        public RuleItem(RuleItemType type,
                        string key,
                        Counter counter = Counter.None,
                        CounterValue counterValue = null,
                        IEnumerable<Condition> conditions = null,
                        string localName = null,
                        bool isHead = false,
                        string semanticId = null
            )
        {
            Type = type;
            Key = key;
            Counter = counter;
            CounterValue = counterValue;
            if (conditions!=null)
                _conditions = conditions.ToList();
            LocalName = localName;
            IsHead = isHead;
            SemanticId = semanticId;

        }
        public bool IsHead { get; set; }
        public RuleItemType Type { get; set; }
        public string Key { get; set; }
        public Counter Counter { get; set; }
        public IEnumerable<Condition> Conditions => _conditions;
        public string LocalName { get; set;}
        public CounterValue CounterValue { get; set;}
        public string SemanticId { get; set; }
        public bool HasFullKey => Key.Contains(".");
        public bool HasConditions => Conditions != null && Conditions.Any();
        public bool HasLocalName => LocalName != null && !string.IsNullOrEmpty(LocalName);
        public bool HasCounter => Counter != Counter.None;
        public void AddCondition(Condition cond)
        {
            _conditions.Add(cond);
        }

        public void RemoveCondition(Condition cond)
        {
            _conditions.Remove(cond);
        }
        public RuleItem Copy()
        {
            return new RuleItem(Type, Key, Counter, CounterValue, Conditions, LocalName, IsHead, SemanticId);
        }

    }
}