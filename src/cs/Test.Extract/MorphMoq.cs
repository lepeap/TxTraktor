using System.Collections.Generic;
using System.Collections.ObjectModel;
using NSubstitute;
using TxTraktor;
using TxTraktor.Morphology;

namespace TxtTractor.Test.Extract
{
    public class MorphMoqBuilder
    {
        private readonly Dictionary<string, MorphInfo[]> _morphDict = new Dictionary<string, MorphInfo[]>();

        public MorphMoqBuilder()
        {
            Result = Substitute.For<IMorphAnalizer>();
            Result
                .When(x => x.SetMorphInfo(Arg.Any<Token[]>()))
                .Do(x =>
                {
                    foreach (var token in (Token[])x.Args()[0])
                    {
                        var key = token.Text.ToLower();
                        if (_morphDict.ContainsKey(key))
                        {
                            token.Morphs = _morphDict[key];
                        }
                    }
                });
        }
        
        public IMorphAnalizer Result { get; }
        
        public void AddMorphData(string word, Dictionary<string, string> gramDic, string lemma=null)
        {
            if (lemma == null)
            {
                lemma = word;
            }
            
            var gDic = new ReadOnlyDictionary<string,string>(gramDic);
            var mi = new MorphInfo(lemma, new ReadOnlyDictionary<string, string>(gDic));
            _morphDict[word.ToLower()] = new[] {mi};
        }
    }
}