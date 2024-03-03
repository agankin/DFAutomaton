using Optional;

namespace DFAutomaton;

internal readonly record struct AutomatonState<TTransition, TState> where TTransition : notnull
{
    private Option<StateValue, AutomatonError<TTransition, TState>> StateValueOrError { get; init; }

    public static AutomatonState<TTransition, TState> AtState(State<TTransition, TState> state, TState value)
    {
        var stateValue = new StateValue(state, value);
        return new() { StateValueOrError = stateValue.Some<StateValue, AutomatonError<TTransition, TState>>() };
    }
    
    public static AutomatonState<TTransition, TState> AtError(AutomatonError<TTransition, TState> error) =>
        new() { StateValueOrError = Option.None<StateValue, AutomatonError<TTransition, TState>>(error) };

    public AutomatonState<TTransition, TState> FlatMap(Func<StateValue, AutomatonState<TTransition, TState>> mapStateValue)
    {
        return StateValueOrError.Match(
            mapStateValue,
            AutomatonState<TTransition, TState>.AtError);
    }

    public Option<TState, AutomatonError<TTransition, TState>> GetAcceptedValueOrError()
    {
        return StateValueOrError.FlatMap(state => state.State.Type == StateType.Accepted
            ? state.Value.Some<TState, AutomatonError<TTransition, TState>>()
            : Option.None<TState, AutomatonError<TTransition, TState>>(GetAccpetedNotReachedError()));
    }

    private static AutomatonError<TTransition, TState> GetAccpetedNotReachedError() => new(
        AutomatonErrorType.AcceptedNotReached,
        Option.None<State<TTransition, TState>>(),
        Option.None<TTransition>(),
        Option.None<Exception>()
    );

    public readonly record struct StateValue(
        State<TTransition, TState> State,
        TState Value
    );
}

internal record AutomatonState
{
    public static AutomatonState<TTransition, TState> State<TTransition, TState>(State<TTransition, TState> state, TState stateValue)
        where TTransition : notnull
    {
        return AutomatonState<TTransition, TState>.AtState(state, stateValue);
    }
}