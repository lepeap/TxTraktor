using System.Collections.Generic;
using System.Linq;
using System.Text;
using TxTraktor.Parse.Forest;

namespace TxTraktor.Compile.Validation
{
    internal class SoglValidator : IValidator
    {
        private int[] _indexes;
        private string _gram;
        
        public SoglValidator(string gram, IEnumerable<int> indexes)
        {
            _gram = gram;
            _indexes = indexes.ToArray();
        }
        
        public bool Validate(IEnumerable<NodeBase> nodes)
        {
            int i = 0;
            string[] vals = null;
            foreach (var node in nodes)
            {
                if (_indexes.Contains(i))
                {
                    var curVals = node.Token
                                      .Morphs
                                      .Select(x => x.GetGramValue(_gram))
                                      .Where(x => x != null)
                                      .ToArray();

                    if (vals == null)
                        vals = curVals;
                    else
                        vals = vals.Intersect(curVals).ToArray();
                }

                if (vals != null && !vals.Any())
                    return false;

                i++;

            }
            return vals != null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var index in _indexes)
            {
                sb.Append(index.ToString());
                sb.Append(",");
            }
            
            return $"{_gram} - {sb}";
        }
    }
}