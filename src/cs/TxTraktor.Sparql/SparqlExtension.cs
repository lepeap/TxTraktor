using System.Collections.Generic;
using System.IO;
using TxTraktor.Extension;
using TxTraktor.Extract;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Query.Datasets;

namespace TxTraktor.Sparql
{
    public class SparqlExtension : IExtension
    {
        private ISparqlQueryProcessor _queryProc;
        private SparqlQueryParser _sparqlQueryParser;
        public SparqlExtension(ISparqlQueryProcessor queryProc)
        {
            _queryProc = queryProc;
            _sparqlQueryParser =  new SparqlQueryParser();
        }
        public string Name => "spql";
        public IEnumerable<Dictionary<string, string>> Process(string query)
        {
            var parsedQuery = _sparqlQueryParser.ParseFromString(query);
            var resObj = _queryProc.ProcessQuery(parsedQuery);
            if (resObj is SparqlResultSet results)
                foreach (var result in results)
                {
                    var resDic = new Dictionary<string, string>();
                    foreach (var item in result)
                    {
                        string value;
                        if (item.Value is LiteralNode node)
                            value = node.Value;
                        else
                            value = item.Value.ToString();
                        resDic[item.Key] = value;
                    }
                    yield return resDic;
                }
            else
                throw new ExtensionException(Name, "Wrong query type");

        }

        public static SparqlExtension GetExtensionFromDir(string path, string filesExt="ttl")
        {
            var graph = new Graph();
            foreach (var file in Directory.GetFiles(path, $"*.{filesExt}", SearchOption.AllDirectories))
            {
                graph.LoadFromFile(file);
            }
            ISparqlDataset ds = new InMemoryDataset(graph);
            var sparqlProcessor = new LeviathanQueryProcessor(ds);
            return new SparqlExtension(sparqlProcessor);
        }
        
        
    }
}