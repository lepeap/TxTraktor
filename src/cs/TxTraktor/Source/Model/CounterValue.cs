namespace TxTraktor.Source.Model
{
    internal class CounterValue
    {
        public CounterValue(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
        public int MinValue { get; private set; }
        public int MaxValue { get; private set; }
    }
}