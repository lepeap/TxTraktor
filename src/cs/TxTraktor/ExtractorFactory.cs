using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private ExtractorSettings _settings;
        private IEnumerable<IExtension> _extensions;
        private IMorphAnalizer _morph;
        public ExtractorFactory(ExtractorSettings settings, 
                                IEnumerable<IExtension> extensions = null,
                                IMorphAnalizer morph = null)
        {
            _settings = settings;
            _extensions = extensions;
            _morph = morph ?? new RuMorphAnalizer();
        }
        
        public static IExtractor Create(string rules, 
                                        ExtractorSettings settings, 
                                        IEnumerable<IExtension> extensions = null,
                                        IMorphAnalizer morph = null)
        {
            var grammar = _createMainGrammar(rules, settings.Language);
            settings.MainGrammar = grammar;
            return Create(settings, extensions, morph);
        }

        public static IExtractor Create(ExtractorSettings settings,
                                        IEnumerable<IExtension> extensions = null,
                                        IMorphAnalizer morph = null)
        {
            return new ExtractorFactory(settings, extensions, morph).CreateExtractor();
        }

        private static string _createMainGrammar(string rules, Language language)
        {
            return $"grammar Main; lang {language.GetTextKey()}; {rules}";
        }

        public virtual IGrammarRepository GrammarRepository
        {
            get
            {
                return new FsGrammarRepository(_settings.GrammarsDirPath, _settings.GrammarsExtension);
            }
        }

        public virtual ITokenizer Tokenizer => new WordPunctTokenizer();

        public IExtractor CreateExtractor()
        {
            var tokenizer = Tokenizer;
            var logger = _settings.Logger ?? new MoqLogger();
            var gramRep = GrammarRepository;

            var gramCompiler = new GrammarCompiler(tokenizer, _morph, _extensions);
            var startTerminalsCreator = new StartTerminalsCreator(_settings);

            var srcGrams = new List<(string key, string src)>();
            if (!string.IsNullOrWhiteSpace(_settings.MainGrammar))
                srcGrams.Add((key: "main", src: _settings.MainGrammar));
            
            if (!string.IsNullOrWhiteSpace(_settings.GrammarsDirPath)) 
                srcGrams.AddRange(gramRep.GetAll());
            
            var grams = _getGrammars(logger, srcGrams, _settings.Language);
            var rules = gramCompiler.Compile(grams, _settings.Variables);
            var startRules = startTerminalsCreator.Create(rules);
            var parser = new EarleyParser(startRules, logger);
            var extractor = new Extractor(tokenizer, _morph, parser, _settings, logger);
            return extractor;
        }

        private IEnumerable<Grammar> _getGrammars(ILogger logger, 
            IEnumerable<(string key, string src)> srcGrams,
            Language lang)
        {
            var gramParser = new GrammarParser(logger);
            var errorGrams = new Dictionary<string, string[]>();
            var grams = new List<Grammar>();
            foreach (var srcGram in srcGrams)
            {
                var gram = gramParser.Parse(srcGram.key, srcGram.src);
                
                if (gram.HasErrors)
                    errorGrams[srcGram.key] = gram.Errors;
                
                else if (gram.Language != lang)
                {
                    logger.Warning($"Ignore grammar '{gram.Name}'. Grammar language != current language.");
                }
                else
                    grams.Add(gram);
            }

            if (errorGrams.Any())
            {
                var sb = new StringBuilder("Parsing errors in grammars.");
                sb.Append(Environment.NewLine);
                foreach (var kp in errorGrams)
                {
                    sb.Append($"Grammar '{kp.Key}':");
                    sb.Append(Environment.NewLine);
                    foreach (var error in kp.Value)
                    {
                        sb.Append($"\t{error}");
                        sb.Append(Environment.NewLine);
                    }
                }
                throw new GrammarSourceParsingException(sb.ToString());
            }

            return grams;
        }

    }
}