using System.Collections.Generic;
using NUnit.Framework;
using TxTraktor;
using TxTraktor.Extract;

namespace Test.Extract
{
    [Parallelizable(ParallelScope.All)]
    public class List
    {
        [Test]
        public void TemplateListStaticValues()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test={123,true}] -> \"тест\";",
                new []{
                    new ExtractionDic("Test.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue(new List<ExtractionValue>()
                        {
                           new ExtractionValue(123, ValueType.Int),
                           new ExtractionValue(true, ValueType.Bool)
                        }
                        ,ValueType.List)}
                    }
                }
            );
        }
        
        [Test]
        public void TemplateListStaticValuesWithRef()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test={$name,$0}] -> \"тест\" as name ;",
                new []{
                    new ExtractionDic("Test.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue(new List<ExtractionValue>()
                            {
                                new ExtractionValue("тест", ValueType.String),
                                new ExtractionValue("тест", ValueType.String)
                            }
                            ,ValueType.List)}
                    }
                }
            );
        }
        
        [Test]
        public void TemplateListStaticValuesWithNullRef()
        {
            Checker.Check(
                "тест 1234.",
                "S[Test={$name,$1}] -> \"тест\" as name ;",
                new []{
                    new ExtractionDic("Test.S", "тест", 0)
                    {
                        {"Test", new ExtractionValue(new List<ExtractionValue>()
                            {
                                new ExtractionValue("тест", ValueType.String)
                            }
                            ,ValueType.List)}
                    }
                }
            );
        }
        
        [Test]
        public void SimpleCounterPlus()
        {
            Checker.Check(
                "1 2 3 4",
                "S[Test=$name] -> r\"\\d\"+ as name ;",
                new []{
                    new ExtractionDic("Test.S", "1 2 3 4", 0)
                    {
                        {"Test", new ExtractionValue(new List<ExtractionValue>()
                            {
                                new ExtractionValue("1", ValueType.String),
                                new ExtractionValue("2", ValueType.String),
                                new ExtractionValue("3", ValueType.String),
                                new ExtractionValue("4", ValueType.String)
                            }
                            ,ValueType.List)}
                    }
                },
                new ExtractorSettings() { RulesToExtract = new []{"Test.S"}}
            );
        }
        
        [Test]
        public void SimpleCounterStar()
        {
            Checker.Check(
                "1 2 3 4 .",
                "S[Test=$name] -> r\"\\d\"* as name ;",
                new []{
                    new ExtractionDic("Test.S", "1 2 3 4", 0)
                    {
                        {"Test", new ExtractionValue(new List<ExtractionValue>()
                            {
                                new ExtractionValue("1", ValueType.String),
                                new ExtractionValue("2", ValueType.String),
                                new ExtractionValue("3", ValueType.String),
                                new ExtractionValue("4", ValueType.String)
                            }
                            ,ValueType.List)}
                    }
                },
                new ExtractorSettings() { RulesToExtract = new []{"Test.S"}}
            );
        }
        
        [Test]
        public void CounterWithTemplatePlus()
        {
            Checker.Check(
                "1 2 3 4",
                "S1 -> r\"\\d\" as test ;"+
                      "S[Test=$name] -> S1+ as name ;",
                new []{
                    new ExtractionDic("Test.S", "1 2 3 4", 0)
                    {
                        {"Test", new ExtractionValue(new List<ExtractionValue>()
                            {
                                new ExtractionValue(
                                    new ExtractionDic("Test.S1", "1", 0)
                                    {
                                        {"Test", new ExtractionValue("1", ValueType.String)}
                                    }
                                    , ValueType.Dictionary),
                                new ExtractionValue(
                                    new ExtractionDic("Test.S1", "2", 2)
                                    {
                                        {"Test", new ExtractionValue("2", ValueType.String)}
                                    }
                                    , ValueType.Dictionary),
                                new ExtractionValue(
                                    new ExtractionDic("Test.S1", "3", 4)
                                    {
                                        {"Test", new ExtractionValue("3", ValueType.String)}
                                    }
                                    , ValueType.Dictionary),
                                new ExtractionValue(
                                    new ExtractionDic("Test.S1", "4", 6)
                                    {
                                        {"Test", new ExtractionValue("4", ValueType.String)}
                                    }
                                    , ValueType.Dictionary),
                            }
                            ,ValueType.List)}
                    }
                },
                new ExtractorSettings() { RulesToExtract = new []{"Test.S"}}
            );
        }
        
        [Test]
        public void SimpleCounterNumber()
        {
            Checker.Check(
                "1 2 3 4",
                "S[Test=$name] -> r\"\\d\"{1,4} as name ;",
                new []{
                    new ExtractionDic("Test.S", "1 2 3 4", 0)
                    {
                        {"Test", new ExtractionValue(new List<ExtractionValue>()
                            {
                                new ExtractionValue("1", ValueType.String),
                                new ExtractionValue("2", ValueType.String),
                                new ExtractionValue("3", ValueType.String),
                                new ExtractionValue("4", ValueType.String)
                            }
                            ,ValueType.List)}
                    }
                },
                new ExtractorSettings() { RulesToExtract = new []{"Test.S"}}
            );
        }

        [Test]
        public void SameLocalNameList1()
        {
            Checker.Check(
                "1 2 3 4",
                "S[Test=$name] -> \"1\" as name \"2\" as name ;",
                new []{
                    new ExtractionDic("Test.S", "1 2", 0)
                    {
                        {"Test", new ExtractionValue(new List<ExtractionValue>()
                            {
                                new ExtractionValue("1", ValueType.String),
                                new ExtractionValue("2", ValueType.String)
                            }
                            ,ValueType.List)}
                    }
                },
                new ExtractorSettings() { RulesToExtract = new []{"Test.S"}}
            );
        }
        
        
        [Test]
        public void SameLocalNameList2()
        {
            Checker.Check(
                "1 2 3 4",
                "S[Test=$name] -> \"1\" as name \"2\" as name \"3\" as name ;",
                new []{
                    new ExtractionDic("Test.S", "1 2 3", 0)
                    {
                        {"Test", new ExtractionValue(new List<ExtractionValue>()
                            {
                                new ExtractionValue("1", ValueType.String),
                                new ExtractionValue("2", ValueType.String),
                                new ExtractionValue("3", ValueType.String)
                            }
                            ,ValueType.List)}
                    }
                },
                new ExtractorSettings() { RulesToExtract = new []{"Test.S"}}
            );
        }
        
    }
}