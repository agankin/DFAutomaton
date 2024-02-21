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
    /// <summary>
    /// Creates <see cref="Automaton{TTransition, TState}"/>.
    /// </summary>
    /// <param name="start">The start state of an automaton.</param>
    public Automaton(IState<TTransition, TState> start) => Start = start;

    /// <summary>
    /// The start state of the automaton.
    /// </summary>
    public IState<TTransition, TState> Start { get; }

    /// <summary>
    /// Contains a current transition at the run time.
    /// </summary>
    public static ThreadLocal<AutomatonTransition<TTransition, TState>> CurrentTransition = new();

    /// <summary>
    /// Runs the automaton over a sequence of transitions reducing the provided start value.
    /// </summary>
    /// <param name="startValue">A start value.</param>
    /// <param name="transitions">A sequence of transitions.</param>
    /// <returns>Some with a reduced value after applying all transitions or None with an error occured.</returns>
    public Option<TState, AutomatonError<TTransition, TState>> Run(TState startValue, IEnumerable<TTransition> transitions)
    {
        var initialState = AutomatonState.State(Start, startValue);
        var transitionsEnumerator = new TransitionsEnumerator<TTransition>(transitions);

        CurrentTransition.Value = new(transitionsEnumerator.YieldNext);
        
        var result = transitionsEnumerator.ToEnumerable()
            .Aggregate(
                initialState,
                (state, transition) => state.FlatMap(stateValue => Transit(stateValue, transition)))
            .GetValueOrError();

        CurrentTransition.Value = null!;

        return result;
    }

    private AutomatonState<TTransition, TState> Transit(AutomatonState<TTransition, TState>.StateValue stateValue, TTransition transition)
    {
        var state = stateValue.State;

        return state[transition].Match(
            stateTransition => Transit(stateValue, stateTransition, transition),
            () => GetTransitionNotFoundError(state, transition));
    }

    private AutomatonState<TTransition, TState> Transit(
        AutomatonState<TTransition, TState>.StateValue stateValue,
        Transition<TTransition, TState> stateTransition,
        TTransition transition)
    {
        var (nextStateOption, reduce) = stateTransition;
        CurrentTransition.Value.TransitsTo = nextStateOption;

        var nextStateValue = reduce(stateValue.Value, transition);

        return nextStateOption.Else(CurrentTransition.Value.DynamiclyGoToState)
            .Map(nextState => AutomatonState<TTransition, TState>.AtState(nextState, nextStateValue))
            .ValueOr(() => GetTransitionNotFoundError(stateValue.State, transition));
    }

    private static AutomatonState<TTransition, TState> GetTransitionNotFoundError(IState<TTransition, TState> state, TTransition transition)
    {
        var errorType = state.Type == StateType.Accepted
            ? AutomatonErrorType.TransitionFromAccepted
            : AutomatonErrorType.TransitionNotExists;
        var error = new AutomatonError<TTransition, TState>(errorType, state, transition);
        
        return AutomatonState<TTransition, TState>.AtError(error);
    }
}