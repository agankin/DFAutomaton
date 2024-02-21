using Optional;

namespace DFAutomaton;

internal readonly record struct AutomatonState<TTransition, TState> where TTransition : notnull
{
    private Option<StateValue, AutomatonError<TTransition, TState>> StateValueOrError { get; init; }

    public static AutomatonState<TTransition, TState> AtState(IState<TTransition, TState> state, TState value)
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

    public Option<TState, AutomatonError<TTransition, TState>> GetValueOrError() => StateValueOrError.Map(state => state.Value);

    public readonly record struct StateValue(
        IState<TTransition, TState> State,
        TState Value
    );
}

internal record AutomatonState
{
    public static AutomatonState<TTransition, TState> State<TTransition, TState>(IState<TTransition, TState> state, TState stateValue)
        where TTransition : notnull
    {
        return AutomatonState<TTransition, TState>.AtState(state, stateValue);
    }
}