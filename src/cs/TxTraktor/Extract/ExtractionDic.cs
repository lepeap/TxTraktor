using System.Collections;
using System.Collections.Generic;

namespace TxTraktor.Extract
{
    public class ExtractionDic : IEnumerable<KeyValuePair<string, ExtractionValue>>
    {
        private Dictionary<string, ExtractionValue> _dic = new Dictionary<string, ExtractionValue>();

        public ExtractionDic(string name, string text, int startPosition)
        {
            Name = name;
            Text = text;
            StartPosition = startPosition;
        }
        
        public string Name { get;  }
        
        public string Text { get;  }
        
        public int StartPosition { get; }
        
        public int EndPosition => StartPosition + Text.Length;

        public IEnumerator<KeyValuePair<string, ExtractionValue>> GetEnumerator()
        {
            return _dic.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T GetValue<T>(string key)
        {
            return _dic[key].GetValue<T>();
        }

        public bool ContainsKey(string key)
        {
            return _dic.ContainsKey(key);
        }

        public void Add(string key, ExtractionValue value)
        {
            _dic[key] = value;
        }

        public ExtractionValue this[string key]
        {
            get => _dic[key];
            set => _dic[key] = value;
        }
        
        public int Count => _dic.Count;
        
        public ExtractionDic Clone()
        {
            var newDic = new ExtractionDic(Name, Text, StartPosition);
            foreach (var kp in this)
            {
                if (kp.Value.Type == ValueType.Dictionary)
                {
                    var extDic = (ExtractionDic) kp.Value.Value;
                    extDic = extDic.Clone();
                    newDic.Add(kp.Key, new ExtractionValue(extDic, ValueType.Dictionary));
                }
                else
                {
                    newDic.Add(kp.Key, kp.Value);
                }
            }

            return newDic;
        }

        public override string ToString()
        {
            return $"{Name} '{Text}' <{string.Join(",", _dic.Keys)}>";
        }
    }
}