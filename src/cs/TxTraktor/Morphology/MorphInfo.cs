using System.Collections.Generic;

namespace TxTraktor.Morphology
{
    public class MorphInfo
    {
        private IReadOnlyDictionary<string, string> _grams;
        public MorphInfo(string lemma, IReadOnlyDictionary<string, string> grams)
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