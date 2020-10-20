using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
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
        private readonly ExtractorSettings _settings;
        private readonly IEnumerable<IExtension> _extensions;
        private readonly IMorphAnalizer _morph;
        private readonly ILoggerFactory _loggerFactory;
        public ExtractorFactory(ExtractorSettings settings,
                                ILoggerFactory loggerFactory = null,
                                IEnumerable<IExtension> extensions = null,
                                IMorphAnalizer morph = null)
        {
            _settings = settings;
            _loggerFactory = loggerFactory;
            _extensions = extensions;
            _morph = morph ?? new RuMorphAnalizer();
        }
        
        public static IExtractor Create(string rules, 
                                        ExtractorSettings settings,
                                        ILoggerFactory loggerFactory = null,
                                        IMorphAnalizer morph = null,
                                        IEnumerable<IExtension> extensions = null)
        {
            var grammar = _createMainGrammar(rules, settings.Language);
            settings.MainGrammar = grammar;
            return Create(settings, 
                          loggerFactory,
                          extensions, 
                          morph);
        }

        public static IExtractor Create(ExtractorSettings settings,
                                        ILoggerFactory loggerFactory = null,
                                        IEnumerable<IExtension> extensions = null,
                                        IMorphAnalizer morph = null)
        {
            return new ExtractorFactory(settings, 
                                        loggerFactory, 
                                        extensions, 
                                        morph).CreateExtractor();
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
            var loggerFactory = _loggerFactory;
            var gramRep = GrammarRepository;

            var gramCompiler = new GrammarCompiler(tokenizer, _morph, _extensions);
            var startTerminalsCreator = new StartTerminalsCreator(_settings);

            var srcGrams = new List<(string key, string src)>();
            if (!string.IsNullOrWhiteSpace(_settings.MainGrammar))
            {
                srcGrams.Add((key: "main", src: _settings.MainGrammar));
            }

            if (!string.IsNullOrWhiteSpace(_settings.GrammarsDirPath))
            {
                srcGrams.AddRange(gramRep.GetAll());
            }
            
            var grams = _getGrammars(loggerFactory, srcGrams, _settings.Language);
            var rules = gramCompiler.Compile(grams, _settings.Variables);
            var startRules = startTerminalsCreator.Create(rules);
            var parserLogger = loggerFactory?.CreateLogger<IChartParser>();
            var parser = new EarleyParser(startRules, parserLogger);
            var extractorLogger = loggerFactory?.CreateLogger<IExtractor>();
            var extractor = new Extractor(tokenizer, _morph, parser, _settings, extractorLogger);
            return extractor;
        }

        private IEnumerable<Grammar> _getGrammars(ILoggerFactory loggerFactory, 
            IEnumerable<(string key, string src)> srcGrams,
            Language lang)
        {
            var logger = loggerFactory?.CreateLogger<IGrammarParser>();
            var gramParser = new GrammarParser(logger);
            var errorGrams = new Dictionary<string, string[]>();
            var grams = new List<Grammar>();
            foreach (var srcGram in srcGrams)
            {
                var gram = gramParser.Parse(srcGram.key, srcGram.src);
                if (gram.HasErrors)
                {
                    errorGrams[srcGram.key] = gram.Errors;
                }

                else if (gram.Language != lang)
                {
                    logger?.LogWarning($"Ignore grammar '{gram.Name}'. Grammar language != current language.");
                }
                else
                {
                    grams.Add(gram);
                }
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