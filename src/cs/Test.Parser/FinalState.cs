namespace TxtTractor.Test.Parser
{
    public class FinalState
    {
        public FinalState(string text, string ruleName, int startColumn, int endColumn)
        {
            Text = text;
            RuleName = ruleName;
            StartColumn = startColumn;
            EndColumn = endColumn;
        }
        public string Text { get; }
            
        public string RuleName { get; }
        public int StartColumn { get; }
        public int EndColumn { get; }
    }
}