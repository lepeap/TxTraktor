using System.Collections.Generic;
using System.Linq;

namespace TxTraktor.Parse
{
    internal class Chart
    {
        private Column[] _columns;
        public Chart(IEnumerable<Token> tokens)
        {
            var cols = tokens.Select((t, i) => new Column(i, t))
                             .ToList();
            cols.Add(new Column(cols.Count, null));
            _columns = cols.ToArray();
        }
        public Chart(IEnumerable<Column> columns)
        {
            _columns = columns.ToArray();
        }
    
        public IEnumerable<Column> Columns => _columns;
        public IEnumerable<State> FlatStates => _columns.SelectMany(x => x.States)
                                                        .OrderBy(s => s.StartColumnIndex)
                                                        .ThenBy(s => s.EndColumnIndex);
        public int ColumnsCount => _columns.Length;
        public Column this[int index] => _columns[index];


        public string GetTokensText(int startColumn, int endColumn)
        {
            return string.Join(
                " ",
                  Columns.Where(c => c.Index >= startColumn && c.Index < endColumn)
                                .OrderBy(c => c.Index)
                                .Select(c => c.Token.Text)
                );

        }
        

        
        public Chart GetOnyCompletedChart()
        {
            return new Chart(_columns.Select(c=> new Column(c.Index, c.Token, c.States.Where(s=>s.IsCompleted))));
        }
    }
}