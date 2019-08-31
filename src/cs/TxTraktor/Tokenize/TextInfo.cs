namespace TxTraktor.Tokenize
{
    public class TextInfo
    {
        public TextInfo(int tokensCount, int textLength)
        {
            TokensCount = tokensCount;
            TextLength = textLength;
        }
        public int TextLength { get; set; }
        public int TokensCount { get; set; }
    }
}