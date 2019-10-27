using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TxTraktor;
using TxTraktor.Extension;
using TxTraktor.Extract;
using ValueType = TxTraktor.Extract.ValueType;

namespace Test.Extract
{
    public static class Checker
    {
        public static void Check(string text, 
                                 string rules, 
                                 IEnumerable<ExtractionDic> etalon, 
                                 ExtractorSettings settings = null,
                                 IEnumerable<IExtension> extensions = null,
                                 params string[] rulesToExtract)
        {
            if (settings==null)
                settings = new ExtractorSettings();

            var extractor = ExtractorFactory.Create(rules, settings, extensions);
            var result = extractor.Parse(text, rulesToExtract);
            _check(etalon.ToArray(), result.ToArray());
        }

        private  static void _check(ExtractionDic[] etalonItems, ExtractionDic[] resultItems)
        {
            Assert.AreEqual(etalonItems.Length,
                resultItems.Length,
                "Wrong dictionaries count");

            for (int i = 0; i < etalonItems.Length; i++)
            {
                var etalonItem = etalonItems[i];
                var resultItem = resultItems[i];
                
                _checkDictionary(etalonItem, resultItem);

            }
        }

        private  static void _checkDictionary(ExtractionDic etalonDic, ExtractionDic resultDic)
        {
            
            Assert.AreEqual(etalonDic.Name,
                resultDic.Name,
                $"Wrong name");
            
            Assert.AreEqual(etalonDic.Text,
                resultDic.Text,
                $"Wrong text");

            Assert.AreEqual(etalonDic.StartPosition,
                resultDic.StartPosition,
                $"Wrong start position");
            
            Assert.AreEqual(etalonDic.EndPosition,
                resultDic.EndPosition,
                $"Wrong end position");
            
            Assert.AreEqual(etalonDic.Count,
                resultDic.Count,
                $"Wrong items count in");

            foreach (var etKp in etalonDic)
            {
                    
                Assert.AreEqual(true,
                    resultDic.ContainsKey(etKp.Key),
                    $"Result doesn't contain key '{etKp.Key}'");

                var etItem = etKp.Value;
                var resItem = resultDic[etKp.Key];
                
                Assert.AreEqual(etItem.Type,
                    resItem.Type,
                    $"Wrong item type with key '{etKp.Key}'");
                
                Assert.AreEqual(etItem.SemanticId,
                    resItem.SemanticId,
                    $"Wrong item semanticId with key '{etKp.Key}'");

                _checkValue(etItem, resItem, etKp.Key);

            }
        }

        private static void _checkValue(ExtractionValue etItem, ExtractionValue resItem, string key)
        {
            if (etItem.Type==ValueType.Dictionary)
                _checkDictionary(etItem.GetValue<ExtractionDic>(), resItem.GetValue<ExtractionDic>());
            else if(etItem.Type==ValueType.Float)
                Assert.That(
                    (float) etItem.Value, 
                    Is.EqualTo((float) resItem.Value).Within(0.0001),
                    $"Wrong item value with key '{key}'"
                );
            else if (etItem.Type == ValueType.List)
            {
                var etList = etItem.GetValue<List<ExtractionValue>>();
                var rezList = resItem.GetValue<List<ExtractionValue>>();
                
                Assert.AreEqual(etList.Count,
                    rezList.Count,
                    $"Wrong list items count with key '{key}'");

                for (int i = 0; i < etList.Count; i++)
                {
                    _checkValue(etList[i], rezList[i], $"{key}_{i}");
                }

            }
            else
                Assert.AreEqual(etItem.Value,
                    resItem.Value,
                    $"Wrong item value with key '{key}'");
        }

    }
}