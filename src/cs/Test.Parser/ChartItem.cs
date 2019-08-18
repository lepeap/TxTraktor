namespace TxtTractor.Test.Parser
{
    public class ChartItem
    {
        public ChartItem(string text, int startColumn, int endColumn)
        {
            Text = text;
            StartColumn = startColumn;
            EndColumn = endColumn;
        }
        public string Text { get; private set; }
        public int StartColumn { get; private set; }
        public int EndColumn { get; private set; }
    }
}