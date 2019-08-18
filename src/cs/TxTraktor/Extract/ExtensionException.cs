namespace TxTraktor.Extract
{
    public class ExtensionException : ExtractionException
    {
        public ExtensionException(string extensionType, 
                                  string message) 
            : base($"Extension '{extensionType}' caused error {message}")
        {
        }
    }
}