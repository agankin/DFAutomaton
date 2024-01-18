using Optional;

namespace DFAutomaton;

internal readonly record struct AutomatonRunState<TTransition, TState> where TTransition : notnull
{
    private Option<CurrentState, AutomatonError<TTransition, TState>> Value { get; init; }

    public static AutomatonRunState<TTransition, TState> State(IState<TTransition, TState> state, TState stateValue)
    {
        var currentState = new CurrentState(state, stateValue);
        
        return new()
        {
            Value = Option.Some<CurrentState, AutomatonError<TTransition, TState>>(currentState)
        };
    }
    
    public static AutomatonRunState<TTransition, TState> Error(AutomatonError<TTransition, TState> error) =>
        new()
        {
            Value = Option.None<CurrentState, AutomatonError<TTransition, TState>>(error)
        };

    public AutomatonRunState<TTransition, TState> FlatMap(Func<IState<TTransition, TState>, TState, AutomatonRunState<TTransition, TState>> map)
    {
        return Value.Match(
            value => map(value.State, value.StateValue),
            AutomatonRunState<TTransition, TState>.Error);
    }

    public Option<TState, AutomatonError<TTransition, TState>> StateValueOrError() => Value.Map(state => state.StateValue);

    private readonly record struct CurrentState(
        IState<TTransition, TState> State,
        TState StateValue
    );
}

internal record AutomatonRunState
{
    public static AutomatonRunState<TTransition, TState> State<TTransition, TState>(IState<TTransition, TState> state, TState stateValue)
        where TTransition : notnull
    {
        return AutomatonRunState<TTransition, TState>.State(state, stateValue);
    }
    
    public static AutomatonRunState<TTransition, TState> Error<TTransition, TState>(AutomatonError<TTransition, TState> error)
        where TTransition : notnull
    {
        return AutomatonRunState<TTransition, TState>.Error(error);
    }
}