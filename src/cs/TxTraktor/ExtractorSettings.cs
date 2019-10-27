using System.Collections.Generic;

namespace TxTraktor
{
    public class ExtractorSettings
    {
        public string MainGrammar { get; set; }
        public Language Language { get; set; } = Language.Ru;
        public string GrammarsDirPath { get; set; }
        public string GrammarsExtension { get; set; } = "gr";
        public string[] RulesToExtract { get; set; }
        public bool SelectLongest { get; set; } = true;
        public Dictionary<string, string> Variables { get; set; }
        public ILogger Logger { get; set; }
    }
}