using DFAutomaton.Utils;
using PureMonads;

namespace DFAutomaton;

using static AutomatonErrorFactory;

/// <summary>
/// An automaton that can be run over a sequence of transitions transforming a start state value into an accepted state value.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class Automaton<TTransition, TState> where TTransition : notnull
{
    private FrozenStateGraph<TTransition, TState> _stateGraph;
    private Predicate<TState> _isErrorValue;

    internal Automaton(FrozenStateGraph<TTransition, TState> stateGraph, Predicate<TState>? isErrorValue)
    {
        _stateGraph = stateGraph;
        _isErrorValue = isErrorValue ?? (_ => false);
    }

    /// <summary>
    /// Runs the automaton over a sequence of transitions.
    /// </summary>
    /// <param name="startValue">A start value.</param>
    /// <param name="transitions">A sequence of transitions.</param>
    /// <returns>Some with an accepted value after applying all transitions or None with an error occured.</returns>
    public Result<TState, AutomatonError<TTransition, TState>> Run(TState startValue, IEnumerable<TTransition> transitions)
    {
        var transitionsEnumerator = new TransitionsEnumerator<TTransition>(transitions);
        var result = Run(startValue, transitionsEnumerator);
        
        return result;
    }

    private Result<TState, AutomatonError<TTransition, TState>> Run(TState startValue, TransitionsEnumerator<TTransition> transitionsEnumerator)
    {        
        var transitions = transitionsEnumerator.ToEnumerable();

        StateId currentStateId = StateId.StartStateId;
        var currentValue = startValue;

        foreach (var transition in transitions)
        {
            if (currentStateId.GetStateType() == StateType.Accepted)
                return CreateTransitionFromAccepted(GetState(currentStateId), transition);
            
            var nextTransition = _stateGraph.GetTransitionEntry(currentStateId, transition);
            if (!nextTransition.HasValue)
                return CreateTransitionNotExists(GetState(currentStateId), transition);

            var (toStateId, reduce) = nextTransition.ValueOrFailure();
            var reductionResult = reduce(currentValue, transition);

            var nextStateId = toStateId.Or(reductionResult.DynamiclyGoToStateId);
            if (!nextStateId.HasValue)
                return CreateNoNextState(GetState(currentStateId), transition);

            var nextValue = reductionResult.Value;
            if (_isErrorValue(nextValue))
                return CreateReducerError(GetState(currentStateId), transition, nextValue);

            currentStateId = nextStateId.ValueOrFailure();
            currentValue = nextValue;
                
            CopyYieldedTransitions(reductionResult, transitionsEnumerator);
        }

        if (currentStateId.GetStateType() != StateType.Accepted)
            return CreateAcceptedNotReached<TTransition, TState>();
        
        return currentValue;
    }

    private FrozenState<TTransition, TState> GetState(StateId stateId) => _stateGraph[stateId];

    private static void CopyYieldedTransitions(ReductionResult<TTransition, TState> reductionResult, TransitionsEnumerator<TTransition> transitionsEnumerator)
    {
        foreach(var yieldedTransition in reductionResult.YieldedTransitions)
            transitionsEnumerator.YieldNext(yieldedTransition);
    }
}