using DFAutomaton.Utils;
using Optional;
using Optional.Unsafe;

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
    public Option<TState, AutomatonError<TTransition, TState>> Run(TState startValue, IEnumerable<TTransition> transitions)
    {
        var transitionsEnumerator = new TransitionsEnumerator<TTransition>(transitions);
        var result = Run(startValue, transitionsEnumerator);
        
        return result;
    }

    private Option<TState, AutomatonError<TTransition, TState>> Run(TState startValue, TransitionsEnumerator<TTransition> transitionsEnumerator)
    {        
        var transitions = transitionsEnumerator.ToEnumerable();

        var currentState = _start;
        var currentValue = startValue;

        foreach (var transition in transitions)
        {
            if (currentState.Type == StateType.Accepted)
                return Error(TransitionFromAccepted(currentState, transition));
            
            var nextTransition = currentState[transition];
            if (!nextTransition.HasValue)
                return Error(TransitionNotFound(currentState, transition));

            var (toState, reduce) = nextTransition.ValueOrFailure();
            var reductionResult = reduce(currentValue, transition);

            var nextState = toState.Else(reductionResult.DynamiclyGoToState);
            if (!nextState.HasValue)
                return Error(NoNextState(currentState, transition));

            var nextValue = reductionResult.Value;
            if (_isErrorValue(nextValue))
                return Error(ReducerError(currentState, transition, nextValue));

            currentState = nextState.ValueOrFailure();
            currentValue = nextValue;
                
            CopyYieldedTransitions(reductionResult, transitionsEnumerator);
        }

        if (currentState.Type != StateType.Accepted)
            return Option.None<TState, AutomatonError<TTransition, TState>>(AcceptedNotReached<TTransition, TState>());
        
        return currentValue.Some<TState, AutomatonError<TTransition, TState>>();
    }

    private static void CopyYieldedTransitions(ReductionResult<TTransition, TState> reductionResult, TransitionsEnumerator<TTransition> transitionsEnumerator)
    {
        foreach(var yieldedTransition in reductionResult.YieldedTransitions)
            transitionsEnumerator.YieldNext(yieldedTransition);
    }

    private static Option<TState, AutomatonError<TTransition, TState>> Error(AutomatonError<TTransition, TState> error) =>
        Option.None<TState, AutomatonError<TTransition, TState>>(error);
}