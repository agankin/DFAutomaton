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
    private State<TTransition, TState> _start;
    private Predicate<TState> _isErrorValue;

    internal Automaton(State<TTransition, TState> start, Predicate<TState>? isErrorValue)
    {
        _start = start;
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

        var currentState = _start;
        var currentValue = startValue;

        foreach (var transition in transitions)
        {
            if (currentState.Type == StateType.Accepted)
                return TransitionFromAccepted(currentState, transition);
            
            var nextTransition = currentState[transition];
            if (!nextTransition.HasValue)
                return TransitionNotFound(currentState, transition);

            var (toState, reduce) = nextTransition.ValueOrFailure();
            var reductionResult = reduce(currentValue, transition);

            var nextState = toState.Or(reductionResult.DynamiclyGoToState);
            if (!nextState.HasValue)
                return NoNextState(currentState, transition);

            var nextValue = reductionResult.Value;
            if (_isErrorValue(nextValue))
                return ReducerError(currentState, transition, nextValue);

            currentState = nextState.ValueOrFailure();
            currentValue = nextValue;
                
            CopyYieldedTransitions(reductionResult, transitionsEnumerator);
        }

        if (currentState.Type != StateType.Accepted)
            return AcceptedNotReached<TTransition, TState>();
        
        return currentValue;
    }

    private static void CopyYieldedTransitions(ReductionResult<TTransition, TState> reductionResult, TransitionsEnumerator<TTransition> transitionsEnumerator)
    {
        foreach(var yieldedTransition in reductionResult.YieldedTransitions)
            transitionsEnumerator.YieldNext(yieldedTransition);
    }
}