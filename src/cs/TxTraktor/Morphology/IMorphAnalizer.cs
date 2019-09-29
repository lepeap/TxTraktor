namespace TxTraktor.Morphology
{
    internal interface IMorphAnalizer
    {
        string Lemmatize(string word);
        void SetMorphInfo(Token[] tokens);
    }
}