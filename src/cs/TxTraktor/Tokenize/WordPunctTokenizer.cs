using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TxTraktor.Tokenize
{
    public class WordPunctTokenizer : ITokenizer
    {
        private Regex _reg = new Regex(@"[\w\-{IsCyrillic}]+|[\p{P}]|[^\w\-{IsCyrillic}\s]+", RegexOptions.Compiled);


        public IEnumerable<Token> Tokenize(string text)
        {
            int i = 0;

            var matches = _reg.Matches(text);
            var textInfo = new TextInfo(matches.Count, text.Length);
            foreach (Match match in matches)
            {
                yield return new Token(match.Value,
                    i++,
                    match.Index,
                    match.Index + match.Length,
                    textInfo);
            }
        }
    }
}