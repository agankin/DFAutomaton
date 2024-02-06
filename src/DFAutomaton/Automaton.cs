using DFAutomaton.Utils;
using Optional;

namespace DFAutomaton;

/// <summary>
/// Automaton that can be run over a sequence of transitions transforming a start state value into an accepted state value.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class Automaton<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// Creates <see cref="Automaton{TTransition, TState}"/>.
    /// </summary>
    /// <param name="start">A start state of an automaton state graph.</param>
    public Automaton(IState<TTransition, TState> start) => Start = start;

    /// <summary>
    /// The start state of the automaton.
    /// </summary>
    public IState<TTransition, TState> Start { get; }

    /// <summary>
    /// Runs automaton over a sequence of transitions reducing the provided start value.
    /// </summary>
    /// <param name="startValue">Start value.</param>
    /// <param name="transitions">Sequence of transitions.</param>
    /// <returns>Some reduced value after applying all transitions or error occured during the run.</returns>
    public Option<TState, AutomatonError<TTransition, TState>> Run(TState startValue, IEnumerable<TTransition> transitions)
    {
        var initialState = AutomatonRunState.State(Start, startValue);
        var transitionsEnumerator = new TransitionsEnumerator<TTransition>(transitions);

        return transitionsEnumerator.ToEnumerable()
            .Aggregate(initialState, (state, transition) => state.FlatMap(Reduce(transition, transitionsEnumerator.Push)))
            .StateValueOrError();
    }

    private Func<IState<TTransition, TState>, TState, AutomatonRunState<TTransition, TState>> Reduce(
        TTransition transition,
        Action<TTransition> push)
    {
        return (state, stateValue) => state[transition].Match(
            Reduce(state, transition, stateValue, push),
            () => GetTransitionNotFoundError(state, transition));
    }

    private Func<IState<TTransition, TState>.Transition, AutomatonRunState<TTransition, TState>> Reduce(
        IState<TTransition, TState> state,
        TTransition transition,
        TState stateValue,
        Action<TTransition> push)
    {
        return stateTransition =>
        {
            var (kind, nextStateOption, reduce) = stateTransition;
            var automatonTransition = new AutomatonTransition<TTransition, TState>(stateValue, transition, nextStateOption, push);

            var (nextStateValue, goToStateOption) = reduce(automatonTransition);

            var resultNextStateOption = kind switch {
                TransitionKind.FixedState => nextStateOption,
                TransitionKind.DynamicGoTo => goToStateOption,
                _ => throw new Exception($"Unsupported enum {nameof(TransitionKind)} value: {kind}.")
            };

            return resultNextStateOption
                .Map(nextState => AutomatonRunState<TTransition, TState>.State(nextState, nextStateValue))
                .ValueOr(() => GetTransitionNotFoundError(state, transition));
        };
    }

    private static AutomatonRunState<TTransition, TState> GetTransitionNotFoundError(IState<TTransition, TState> state, TTransition transition)
    {
        var errorType = state.Type == StateType.Accepted
            ? AutomatonErrorType.TransitionFromAccepted
            : AutomatonErrorType.TransitionNotFound;

        var error = new AutomatonError<TTransition, TState>(errorType, state, transition);
        return AutomatonRunState<TTransition, TState>.Error(error);
    }
}