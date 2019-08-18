using System.Collections.Generic;

namespace TxTraktor.Parse
{
    internal class Column
    {
        private Dictionary<int, State> _states;
        private List<int> _keysInOrder;
        public Column(int index, Token token, IEnumerable<State> states = null)
        {
            Index = index;
            Token = token;
            _states = new Dictionary<int, State>();
            _keysInOrder = new List<int>();
            
            if (states == null) 
                return;
            
            foreach (var state in states)
                AddState(state);
        }
        
        public int Index { get; private set; }
        public Token Token { get; private set; }

        public IEnumerable<State> States
        {
            get
            {
                for (int i = 0; i < _keysInOrder.Count; i++)
                {
                    var curKey = _keysInOrder[i];
                    yield return _states[curKey];
                }
            }
        }

        public int StatesCount => _states.Count;

        public void AddState(State state)
        {
            var key = state.GetHashCode();
            if (_states.ContainsKey(key))
            {
                _states[key].AddParents(state.Parents);
            }
            else
            {
                _states[key] = state;
                _keysInOrder.Add(key);
            }
        }
        
        public State this[int index] => _states[_keysInOrder[index]];
        
        public override string ToString()
        {
            return Token != null ? $"{Index} : {Token.Text}" : "Final column";
        }
    }
}