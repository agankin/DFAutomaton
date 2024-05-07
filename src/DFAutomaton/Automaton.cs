using DFAutomaton.Utils;
using Optional;

namespace DFAutomaton;

/// <summary>
/// An automaton that can be run over a sequence of transitions transforming a start state value into an accepted state value.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class Automaton<TTransition, TState> where TTransition : notnull
{
    private State<TTransition, TState> _start;
    private Predicate<TState> _isErrorState;

    internal Automaton(State<TTransition, TState> start, Predicate<TState>? isErrorState)
    {
        _start = start;
        _isErrorState = isErrorState ?? (_ => false);
    }

    /// <summary>
    /// Contains a current transition at runtime.
    /// </summary>
    public static ThreadLocal<AutomatonTransition<TTransition, TState>> CurrentTransition = new();

    /// <summary>
    /// Runs the automaton over a sequence of transitions.
    /// </summary>
    /// <param name="startValue">A start value.</param>
    /// <param name="transitions">A sequence of transitions.</param>
    /// <returns>Some with an accepted value after applying all transitions or None with an error occured.</returns>
    public Option<TState, AutomatonError<TTransition, TState>> Run(TState startValue, IEnumerable<TTransition> transitions)
    {
        var startState = AutomatonState.State(_start, startValue);
        var transitionsEnumerator = new TransitionsEnumerator<TTransition>(transitions);

        var result = Run(transitionsEnumerator, startState);
        return result;
    }

    private Option<TState, AutomatonError<TTransition, TState>> Run(
        TransitionsEnumerator<TTransition> transitionsEnumerator,
        AutomatonState<TTransition, TState> startState)
    {
        CurrentTransition.Value = new(_start.OwningGraph, transitionsEnumerator.YieldNext);
        
        var isErrorAutomatonState = new Predicate<AutomatonState<TTransition, TState>>(
            automatonState => automatonState.MatchValueOrError(
                stateValue => _isErrorState.Invoke(stateValue),
                _ => false
            )
        );
        var transitionsReducer = new SequenceAggregator<TTransition, AutomatonState<TTransition, TState>>(isErrorAutomatonState);
        
        var transitions = transitionsEnumerator.ToEnumerable();
        Func<AutomatonState<TTransition, TState>, TTransition, AutomatonState<TTransition, TState>> reducer =
            (fromState, transition) => fromState.FlatMap(fromStateValue => Transit(fromStateValue, transition));
        
        var result = transitionsReducer
            .Reduce(startState, transitions, reducer)
            .GetAcceptedValueOrError();

        CurrentTransition.Value = null!;

        return result;
    }

    private AutomatonState<TTransition, TState> Transit(
        AutomatonState<TTransition, TState>.StateValue fromStateValue,
        TTransition transition)
    {
        var state = fromStateValue.State;

        return state[transition].Match(
            stateTransition => Transit(fromStateValue, stateTransition, transition),
            () => GetTransitionNotFoundError(state, transition));
    }

    private AutomatonState<TTransition, TState> Transit(
        AutomatonState<TTransition, TState>.StateValue fromStateValue,
        Transition<TTransition, TState> stateTransition,
        TTransition transition)
    {
        var (nextStateOption, reduce) = stateTransition;
        var (fromState, fromValue) = fromStateValue;
        
        CurrentTransition.Value.DynamiclyGoToState = Option.None<State<TTransition, TState>>();
        CurrentTransition.Value.TransitsTo = nextStateOption.Map(state => new ImmutableState<TTransition, TState>(state));
        var nextValue = reduce(fromValue, transition);

        return nextStateOption.Else(CurrentTransition.Value.DynamiclyGoToState)
            .Map(nextState => _isErrorState(nextValue)
                ? AutomatonState<TTransition, TState>.AtError(AutomatonError<TTransition, TState>.ReducerError(fromState, transition, nextValue))
                : AutomatonState<TTransition, TState>.AtState(nextState, nextValue))
            .ValueOr(() => GetTransitionNotFoundError(fromState, transition));
    }

    private static AutomatonState<TTransition, TState> GetTransitionNotFoundError(State<TTransition, TState> fromState, TTransition transition)
    {
        var error = AutomatonError<TTransition, TState>.TransitionNotFound(fromState, transition);
        return AutomatonState<TTransition, TState>.AtError(error);
    }
}