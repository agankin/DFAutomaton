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
    public State<TTransition, TState> _start;

    /// <summary>
    /// Creates <see cref="Automaton{TTransition, TState}"/>.
    /// </summary>
    /// <param name="start">The start state of an automaton.</param>
    internal Automaton(State<TTransition, TState> start) => _start = start;

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
        
        try
        {
            var result = Run(transitionsEnumerator, startState);
            return result;
        }
        catch (Exception ex)
        {
            return Option.None<TState, AutomatonError<TTransition, TState>>(
                AutomatonError<TTransition, TState>.RuntimeError(ex));
        }
    }

    private Option<TState, AutomatonError<TTransition, TState>> Run(
        TransitionsEnumerator<TTransition> transitionsEnumerator,
        AutomatonState<TTransition, TState> startState)
    {
        CurrentTransition.Value = new(_start.OwningGraph, transitionsEnumerator.YieldNext);
        
        var result = transitionsEnumerator.ToEnumerable()
            .Aggregate(
                startState,
                (fromState, transition) => fromState.FlatMap(fromStateValue => Transit(fromStateValue, transition)))
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
        
        CurrentTransition.Value.TransitsTo = nextStateOption.Map(state => new ImmutableState<TTransition, TState>(state));
        var nextStateValue = reduce(fromStateValue.Value, transition);

        return nextStateOption.Else(CurrentTransition.Value.DynamiclyGoToState)
            .Map(nextState => AutomatonState<TTransition, TState>.AtState(nextState, nextStateValue))
            .ValueOr(() => GetTransitionNotFoundError(fromStateValue.State, transition));
    }

    private static AutomatonState<TTransition, TState> GetTransitionNotFoundError(State<TTransition, TState> fromState, TTransition transition)
    {
        var error = AutomatonError<TTransition, TState>.TransitionNotFound(fromState, transition);
        return AutomatonState<TTransition, TState>.AtError(error);
    }
}