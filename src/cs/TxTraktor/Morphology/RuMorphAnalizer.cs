using System.Linq;
using DeepMorphy;

namespace TxTraktor.Morphology
{
    internal class RuMorphAnalizer : IMorphAnalizer
    {
        private readonly MorphAnalyzer _morph = new MorphAnalyzer(withLemmatization: true);

        public string Lemmatize(string word)
        {
            return _morph.Parse(new[] {word}).First().BestTag.Lemma;
        }

        public void SetMorphInfo(Token[] tokens)
        {
            var results = _morph.Parse(tokens.Select(x => x.Text));
            int i = 0;
            foreach (var result in results)
            {
                tokens[i++].Morphs = result.Tags
                    .Select(x => new MorphInfo(x.Lemma, x.GramsDic))
                    .ToArray();
            }
        }
    }
}