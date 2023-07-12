using DFAutomaton.Utils;
using Optional;

namespace DFAutomaton;

public class Automaton<TTransition, TState> where TTransition : notnull
{
    public Automaton(IState<TTransition, TState> start) => Start = start;

    public IState<TTransition, TState> Start { get; }

    public Option<TState, AutomatonError<TTransition, TState>> Run(TState startValue, IEnumerable<TTransition> transitions)
    {
        var initialState = new CurrentState(Start, startValue).Some<CurrentState, AutomatonError<TTransition, TState>>();
        var transitionsEnumerator = new TransitionsEnumerator<TTransition>(transitions);

        return transitionsEnumerator.ToEnumerable()
            .Aggregate(
                initialState,
                (state, transition) => Reduce(transitionsEnumerator.QueueEmited, state, transition))
            .Map(state => state.StateValue);
    }

    private Option<CurrentState, AutomatonError<TTransition, TState>> Reduce(
        Action<TTransition> emitNext,
        Option<CurrentState, AutomatonError<TTransition, TState>> currentState,
        TTransition transition)
    {
        return currentState.FlatMap(Reduce(emitNext, transition));
    }

    private Func<CurrentState, Option<CurrentState, AutomatonError<TTransition, TState>>> Reduce(
        Action<TTransition> emitNext,
        TTransition transition)
    {
        return automatonState =>
        {
            var (state, stateValue) = automatonState;

            return state[transition].Match(
                Reduce(emitNext, stateValue),
                () => Option.None<CurrentState, AutomatonError<TTransition, TState>>(GetErrorForTransitionNotFound(state, transition)));
        };
    }

    private Func<Transition<TTransition, TState, IState<TTransition, TState>>, Option<CurrentState, AutomatonError<TTransition, TState>>> Reduce(
        Action<TTransition> emitNext,
        TState stateValue)
    {
        return automatonNextState =>
        {
            var (nextState, reducer) = automatonNextState;
            var runState = new AutomatonRunState<TTransition, TState>(nextState, emitNext);
            var nextStateValue = reducer(runState, stateValue);
            var nextAutomatonState = new CurrentState(nextState, nextStateValue);

            return nextAutomatonState.Some<CurrentState, AutomatonError<TTransition, TState>>();
        };
    }

    private static AutomatonError<TTransition, TState> GetErrorForTransitionNotFound(IState<TTransition, TState> state, TTransition transition)
    {
        var errorType = state.Type == StateType.Accepted
            ? AutomatonErrorType.TransitionFromAccepted
            : AutomatonErrorType.TransitionNotExists;

        return new(errorType, state, transition);
    }

    private readonly record struct CurrentState(
        IState<TTransition, TState> State,
        TState StateValue
    );
}