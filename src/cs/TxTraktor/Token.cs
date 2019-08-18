using System.Linq;
using TxTraktor.Morphology;
using TxTraktor.Tokenize;

namespace TxTraktor
{
    internal class Token
    {
        private string _lowerText;
        public Token(string text, int index, int startPosition, int endPosition, TextInfo textInfo)
        {
            Text = text;
            Index = index;
            StartPosition = startPosition;
            EndPosition = endPosition;
            TextInfo = textInfo;
        }
        
        public Token(string text) : this(text, 
                                    0, 
                                    0, 
                                    text.Length,
                                     new TextInfo(1, text.Length))
        {
        }
        
        public string Text { get; }

        public string LowerText
        {
            get
            {
                if (_lowerText == null)
                    _lowerText = Text.ToLower();

                return _lowerText;
            }
        }
        
        public int Index { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        
        public TextInfo TextInfo { get; }

        public MorphInfo[] Morphs { get; set; }

        public bool HasMorph => Morphs != null;

        public bool HasLemma(string text)
        {
            return Text==text || (Morphs?.Any(x => x.Lemma == text) ?? false);
        }
        
        public override string ToString()
        {
            return $"{Text}({StartPosition},{EndPosition})";
        }
    }
}