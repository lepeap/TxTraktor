using System.Collections.Generic;
using System.Linq;
using TxTraktor.Compile.Model;
using TxTraktor.Parse.Forest;

namespace TxTraktor.Parse
{
    internal class EarleyParser : IChartParser
    {
        private readonly ILogger _logger;
        private readonly StartTerminal[] _startStartTerminals;
        public EarleyParser(IEnumerable<StartTerminal> startTerminals, ILogger logger)
        {
            _startStartTerminals = startTerminals.ToArray();
            _logger = logger;
        }
        
        
        public Chart Parse(IEnumerable<Token> tokens)
        {
            var chart = new Chart(tokens);
            _logger.Debug(
                "Earley parsing start. Tokens count: {TokensCount}. Start terminals count: {StartTerminalsCount}", 
                chart.ColumnsCount,
                _startStartTerminals.Length
            );
            for (int i = 0; i < chart.ColumnsCount; i++)
            {
                
                var col = chart[i];
                _logger.Debug(
                    "Column start {ColumnIndex}. Token: {TokenText} ({TokenStartPosition}, {TokenEndPosition})", 
                    i, 
                                col.Token?.Text,
                                col.Token?.StartPosition,
                                col.Token?.EndPosition
                    );
                var nextCol = i < chart.ColumnsCount-1 ? chart[i + 1] : null;
                
                if (nextCol!=null)
                    _checkStartRules(col);

                foreach (var state in col.States)
                {
                    if (state.IsCompleted)
                        _complete(state, col);
                    else if (!state.CurrentTerm.IsTerminal)
                        _predict(state, col);
                    else if (nextCol!=null)
                        _scan(state, col, nextCol);
                }
            }

            return chart;
        }
        
        private void _scan(State state, Column col, Column nextCol)
        {
            if (state.CurrentTerm.IsValid(col.Token))
            {
                var term = state.CurrentTerm;
                var leaf = new Leaf(term, 
                                    col.Token, 
                                    state.DotIndex, 
                                    term.LocalName, 
                                    state.CurrentTerm.IsHead,
                                    state.CurrentTerm.SemanticId);
                var newState = new State(state.Rule,
                    state.DotIndex + 1,
                    state.StartColumnIndex,
                    state.Node.Attache(leaf),
                    "SCAN",
                    parents: state.Parents,
                    endColumnIndex: col.Index + 1);
                state.AddChild(newState);
                nextCol.AddState(newState);
                _logger.Debug("Scan state: {State}", newState);
            }
        }
        
        private void _predict(State state, Column col)
        {
            var term = (NonTerminal) state.CurrentTerm;
            foreach (var rl in term.Rules)
            {
                var node = new Node(rl, 
                                    state.DotIndex, 
                                    state.CurrentTerm.LocalName,
                                    state.CurrentTerm.IsHead,
                                    state.CurrentTerm.SemanticId);
                var newState = new State(
                    rl,
                    0,
                    col.Index,
                    node,
                    createOpType: "PREDICT",
                    endColumnIndex: col.Index,
                    parent: state
                );
                col.AddState(newState);
                _logger.Debug("Predict state: {State}", newState);
            }

            if (term.IsNullable)
            {
                var newState = new State(
                    state.Rule,
                    state.DotIndex + 1,
                    state.StartColumnIndex,
                    state.Node.DeepCopy(),
                    createOpType: "PREDICT_NULL",
                    parents: state.Parents,
                    endColumnIndex: col.Index
                );
                col.AddState(newState);
                _logger.Debug("Predict empty state: {State}", newState);
            }
        }
        
        private void _complete(State state, Column col)
        {
            var headToken = state.Node.Head;
            if (!state.Node.IsValid)
            {
                _logger.Debug("State '{State}' is not valid. Ignore complete.", state);
                return;
            }

            foreach (var parent in state.Parents)
            {
                // проверяем условие на родительский нетерминал
                if (parent.CurrentTerm.IsValid(headToken))
                {
                    var node = state.Node.Copy();
                    node.LocalName = parent.CurrentTerm.LocalName;
                    var newState = new State(
                        parent.Rule,
                        parent.DotIndex + 1,
                        startColumnIndex: parent.StartColumnIndex,
                        parent.Node.Attache(node),
                        createOpType: "COMPLETE",
                        parents: parent.Parents,
                        endColumnIndex: col.Index
                    );
                    col.AddState(newState);
                    _logger.Debug("Complete state: {State}", newState);
                }
            }
        }

        private void _checkStartRules(Column col)
        {
            foreach (var srl in _startStartTerminals)
            {
                if (srl.Check(col.Token))
                {
                    foreach (var srlRule in srl.Rules)
                    {
                        var node = new Node(srlRule, 0, semanticId: srlRule[0].SemanticId);
                        var state = new State(
                            srlRule,
                            0,
                            col.Index,
                            node,
                            createOpType: "START_RULE",
                            endColumnIndex: col.Index
                        );
                        col.AddState(state);
                        _logger.Debug("Start state: {State}", state);
                    }
                }
            }
        }
    }
}