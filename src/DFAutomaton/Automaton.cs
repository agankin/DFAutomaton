using DFAutomaton.Utils;
using PureMonads;

namespace DFAutomaton;

using static AutomatonErrorFactory;

/// <summary>
/// Represents an automaton performing state transformations over a sequence of transitions.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
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
    /// <returns>An accepted value or an error occured.</returns>
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
                return TransitionFromAccepted(GetState(currentStateId), transition, currentValue);
            
            var nextTransition = _stateGraph.GetTransitionEntry(currentStateId, transition);
            if (!nextTransition.HasValue)
                return TransitionNotExists(GetState(currentStateId), transition, currentValue);

            var (toStateId, reduce) = nextTransition.ValueOrFailure();
            var reductionResult = reduce(currentValue, transition);

            var nextValue = reductionResult.Value;
            if (_isErrorValue(nextValue))
                return StateError(GetState(currentStateId), transition, nextValue);
            
            var nextStateId = toStateId.Or(reductionResult.DynamiclyGoToStateId);
            if (!nextStateId.HasValue)
                return NoNextState(GetState(currentStateId), transition, currentValue);

            currentStateId = nextStateId.ValueOrFailure();
            currentValue = nextValue;

            YieldNextTransitions(reductionResult, transitionsEnumerator);
        }

        if (currentStateId.GetStateType() != StateType.Accepted)
            return AcceptedNotReached<TTransition, TState>(currentValue);
        
        return currentValue;
    }

    private FrozenState<TTransition, TState> GetState(StateId stateId) => _stateGraph[stateId];

    private static void YieldNextTransitions(ReductionResult<TTransition, TState> reductionResult, TransitionsEnumerator<TTransition> transitionsEnumerator)
    {
        foreach(var yieldedTransition in reductionResult.YieldedTransitions)
            transitionsEnumerator.YieldNext(yieldedTransition);
    }
}