using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TxTraktor.Morphology
{
    public class MorphInfo
    {
        private ReadOnlyDictionary<string, string> _grams;
        public MorphInfo(string lemma, ReadOnlyDictionary<string, string> grams)
        {
            Lemma = lemma;
            _grams = grams;
        }
        
        public string Lemma { get;  }
        
        public IEnumerable<string> Grams => _grams.Values;

        public string GetGramValue(string gramKey)
        {
            if (_grams.ContainsKey(gramKey))
                return _grams[gramKey];

            return null;
        }

        public override string ToString()
        {
            return $"{Lemma} : {string.Join(", ", Grams)}";
        }
    }
}