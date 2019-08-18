using System.Collections.Generic;
using System.Linq;

namespace TxTraktor.Source.Model
{
    internal class Condition
    {
        public Condition()
        {
        }
        public Condition(string key,  IEnumerable<string> values=null, bool negation=false)
        {
            Key = key;
            Negation = negation;
            if (values!=null)
                Values = values.ToArray();
        }

        public Condition(string key, string value, bool negation = false)
        : this(key, new []{value}, negation)
        {}
        
        public Condition(string key, bool negation = false)
            : this(key, (IEnumerable<string>)null, negation)
        {}
        
        public string Key { get;  set; }
        public bool Negation { get;  set; }
        public string[] Values { get;  set; }
    }
}