namespace TxTraktor.Morphology
{
    public interface IMorphAnalizer
    {
        string Lemmatize(string word);
        void SetMorphInfo(Token[] tokens);
    }
}