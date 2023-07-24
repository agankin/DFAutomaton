using DFAutomaton.Utils;
using Optional;

namespace DFAutomaton;

public class Automaton<TTransition, TState> where TTransition : notnull
{
    public Automaton(IState<TTransition, TState> start) => Start = start;

    public IState<TTransition, TState> Start { get; }

    public Option<TState, AutomatonError<TTransition, TState>> Run(TState startValue, IEnumerable<TTransition> transitions)
    {
        var initialState = AutomatonRunState.State(Start, startValue);
        var transitionsEnumerator = new TransitionsEnumerator<TTransition>(transitions);

        return transitionsEnumerator.ToEnumerable()
            .Aggregate(initialState, (state, transition) => state.FlatMap(Reduce(transition, transitionsEnumerator.EmitNext)))
            .StateValueOrError();
    }

    private Func<IState<TTransition, TState>, TState, AutomatonRunState<TTransition, TState>> Reduce(
        TTransition transition,
        Action<TTransition> emitNext)
    {
        return (state, stateValue) => state[transition].Match(
            Reduce(state, transition, stateValue, emitNext),
            () => GetTransitionNotFoundError(state, transition));
    }

    private Func<IState<TTransition, TState>.Transition, AutomatonRunState<TTransition, TState>> Reduce(
        IState<TTransition, TState> state,
        TTransition transition,
        TState stateValue,
        Action<TTransition> emitNext)
    {
        return stateTransition =>
        {
            var (type, nextStateOption, reduce) = stateTransition;
            var automatonState = new AutomatonState<TTransition, TState>(stateValue, nextStateOption, emitNext);

            var (nextStateValue, goToStateOption) = reduce(automatonState);

            var resultNextStateOption = type switch {
                TransitionType.FixedState => nextStateOption,
                TransitionType.DynamicGoTo => goToStateOption,
                _ => throw new Exception($"Unsupported enum {nameof(TransitionType)} value: {type}.")
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
            : AutomatonErrorType.TransitionNotExists;

        var error = new AutomatonError<TTransition, TState>(errorType, state, transition);
        return AutomatonRunState<TTransition, TState>.Error(error);
    }
}