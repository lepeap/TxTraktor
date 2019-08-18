namespace TxTraktor
{
    public interface ILogger
    {
        void Debug(string template, params object[] items);
        void Information(string template, params object[] items);
        void Warning(string template, params object[] items);
        void Error(string template, params object[] items);
    }
}