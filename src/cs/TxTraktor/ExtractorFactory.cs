using System.Collections.Generic;
using System.Linq;
using TxTraktor.Compile;
using TxTraktor.Extension;
using TxTraktor.Extract;
using TxTraktor.Morphology;
using TxTraktor.Parse;
using TxTraktor.Source;
using TxTraktor.Source.Model;
using TxTraktor.Tokenize;

namespace TxTraktor
{
    public class ExtractorFactory
    {
        public static IExtractor Create(ExtractorSettings settings, IEnumerable<IExtension> extensions = null)
        {
            return new ExtractorFactory().CreateExtractor(settings, extensions);
        }

        internal virtual IGrammarRepository _CreateGrammarRepository(ExtractorSettings settings)
        {
            return new FsGrammarRepository(settings.GrammarsDirPath, settings.GrammarsExtension);
        }

        public IExtractor CreateExtractor(ExtractorSettings settings, IEnumerable<IExtension> extensions = null)
        {
            var tokenizer = new WordPunctTokenizer();
            var logger = settings.Logger ?? new MoqLogger();
            var gramRep = _CreateGrammarRepository(settings);
            
            var gramCompiler = new GrammarCompiler(tokenizer, extensions);
            var startTerminalsCreator = new StartTerminalsCreator(settings);
            
            var srcGrams = gramRep.GetAll();
            var grams = _getGrammars(logger, srcGrams, settings.Language);
            
            var rules = gramCompiler.Compile(grams, settings.Variables);
            var startRules = startTerminalsCreator.Create(rules);
            var parser = new EarleyParser(startRules, logger);


            IMorphAnalizer morph = null;
            
            if (settings.Language == Language.Ru)
                morph = new RuMorphAnalizer();

            var extractor = new Extractor(tokenizer, morph, parser, settings, logger);

            return extractor;
        }

        private IEnumerable<Grammar> _getGrammars(ILogger logger, 
            IEnumerable<(string key, string src)> srcGrams,
            Language lang)
        {
            var gramParser = new GrammarParser(logger);
            var errorGrams = new List<string>();
            var grams = new List<Grammar>();
            foreach (var srcGram in srcGrams)
            {
                var gram = gramParser.Parse(srcGram.key, srcGram.src);
                
                if (gram==null)
                    errorGrams.Add(srcGram.key);
                
                else if (gram.Language != lang)
                {
                    logger.Warning($"Ignore grammar '{gram.Name}'. Grammar language != current language.");
                }
                else
                    grams.Add(gram);
            }

            if (errorGrams.Any())
                throw new GrammarSourceParsingException(
                    $"Parsing errors in grammars: {string.Join(",", errorGrams)}"
                );

            return grams;
        }

    }
}