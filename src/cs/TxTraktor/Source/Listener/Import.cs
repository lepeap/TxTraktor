using TxTraktor.Source.Model;

namespace TxTraktor.Source.Listener
{
    internal partial class Main
    {
        private Import Import { get; set; }
        public override void EnterImprt(CfgGramParser.ImprtContext context)
        {
            Import = new Import();
        }

        public override void EnterGrammar_import(CfgGramParser.Grammar_importContext context)
        {
            Import.Type = ImportType.Grammar;
        }

        public override void EnterNonterm_import(CfgGramParser.Nonterm_importContext context)
        {
            Import.Type = ImportType.NonTerminal;
        }

        public override void EnterImp_src_grammar_name(CfgGramParser.Imp_src_grammar_nameContext context)
        {
            Import.Source = context.GetText();
        }

        public override void EnterImp_local_grammar_name(CfgGramParser.Imp_local_grammar_nameContext context)
        {
            Import.LocalName = context.GetText();
        }

        public override void EnterImp_src_nonterminal_name(CfgGramParser.Imp_src_nonterminal_nameContext context)
        {
            Import.Name = context.GetText();
        }

        public override void EnterImp_local_nonterminal_name(CfgGramParser.Imp_local_nonterminal_nameContext context)
        {
            Import.LocalName = context.GetText();
        }

        public override void ExitImprt(CfgGramParser.ImprtContext context)
        {
            if (Import.Type == ImportType.NonTerminal && string.IsNullOrWhiteSpace(Import.LocalName))
            {
                Import.LocalName = Import.Name;
            }
            Grammar.AddImport(Import);
        }
    }
}