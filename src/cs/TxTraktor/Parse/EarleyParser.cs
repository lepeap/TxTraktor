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
        
        public Chart Parse(IEnumerable<Token> tokens, params string[] rulesToExtract)
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
                _logger.Debug("=============================================================================");
                _logger.Debug("=============================================================================");
                _logger.Debug(
                    "Column start {ColumnIndex}. Token: {TokenText} ({TokenStartPosition}, {TokenEndPosition})", 
                    i, 
                                col.Token?.Text,
                                col.Token?.StartPosition,
                                col.Token?.EndPosition);
                
                var nextCol = i < chart.ColumnsCount - 1 ? chart[i + 1] : null;
                if (nextCol!=null)
                    _checkStartRules(col, rulesToExtract);

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
            var term123 = state.CurrentTerm;
            if (state.CurrentTerm.CheckConditions(col.Token))
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
                _logger.Debug("SCAN: Success {State} -> {NewState}", state, newState);
            }
            else
            {
                _logger.Debug("SCAN: Term condition {Term} returned false for token {Token}", state.CurrentTerm, col.Token);
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
                _logger.Debug("PREDICT: {State}", newState);
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
                _logger.Debug("PREDICT: From empty {State}", newState);
            }
        }
        
        private void _complete(State state, Column col)
        {
            var headToken = state.Node.Head;
            if (!state.Node.IsValid)
            {
                _logger.Debug("COMPLETE: State '{State}' is not valid. Ignore complete.", state);
                return;
            }

            foreach (var parent in state.Parents)
            {
                // проверяем условие на родительский нетерминал
                if (parent.CurrentTerm.CheckConditions(headToken))
                {
                    var node = state.Node.Copy();
                    node.LocalName = parent.CurrentTerm.LocalName;
                    var newState = new State(
                        parent.Rule,
                        parent.DotIndex + 1,
                        parent.StartColumnIndex,
                        parent.Node.Attache(node),
                        createOpType: "COMPLETE",
                        parents: parent.Parents,
                        endColumnIndex: col.Index
                    );
                    col.AddState(newState);
                    _logger.Debug("COMPLETE: New state {newState} from {state}", newState, state);
                }
                else
                {
                    _logger.Debug("COMPLETE: Nonterminal condition returned false. Parent {parent} from {state}", parent, state);
                }
            }
        }

        private void _checkStartRules(Column col, params string[] rulesToExtract)
        {
            foreach (var srl in _startStartTerminals)
            {
                if (srl.Check(col.Token))
                {
                    foreach (var srlRule in srl.Rules)
                    {
                        if (rulesToExtract.Any() && !rulesToExtract.Contains(srlRule.Name))
                            continue;
                        
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
                        _logger.Debug("START: {State}", state);
                    }
                }
            }
        }
    }
}