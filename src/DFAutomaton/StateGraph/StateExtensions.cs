using Optional;

namespace DFAutomaton;

public static class StateExtensions
{
    public static Option<IState<TTransition, TState>, StateError> Complete<TTransition, TState>(this State<TTransition, TState> start, BuildConfiguration configuration)
        where TTransition : notnull
    {
        return configuration.ValidateAnyReachesAccepted
            ? start.AsImmutable().ValidateAnyReachAccepted()
            : start.AsImmutable().Some<IState<TTransition, TState>, StateError>();
    }

    public static State<TTransition, TState> ToNewFixedState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        TState newValue)
        where TTransition : notnull
    {
        var reduce = Constant<TTransition, TState>(newValue);
        return current.ToNewFixedState(transition, reduce);
    }

    public static State<TTransition, TState> ToNewFixedState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        ReduceValue<TTransition, TState> reduce)
        where TTransition : notnull
    {
        var newState = StateFactory<TTransition, TState>.SubState(current.GetNextId);
        return current.LinkFixedState(transition, newState, reduce);
    }

    public static State<TTransition, TState> LinkFixedState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        State<TTransition, TState> nextState,
        TState nextValue)
        where TTransition : notnull
    {
        var reduce = Constant<TTransition, TState>(nextValue);
        return current.LinkFixedState(transition, nextState, reduce);
    }

    public static AcceptedState<TTransition, TState> ToNewFixedAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        TState acceptedValue)
        where TTransition : notnull
    {
        var reduce = Constant<TTransition, TState>(acceptedValue);
        return current.ToNewFixedAccepted(transition, reduce);
    }

    public static AcceptedState<TTransition, TState> ToNewFixedAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        ReduceValue<TTransition, TState> reduce)
        where TTransition : notnull
    {
        var acceptedState = StateFactory<TTransition, TState>.Accepted(current.GetNextId);
        current.LinkFixedState(transition, acceptedState, reduce);

        return new AcceptedState<TTransition, TState>(acceptedState);
    }

    public static AcceptedState<TTransition, TState> LinkFixedAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        AcceptedState<TTransition, TState> acceptedState,
        TState acceptedStateValue)
        where TTransition : notnull
    {
        var reduce = Constant<TTransition, TState>(acceptedStateValue);
        return current.LinkFixedAccepted(transition, acceptedState, reduce);
    }

    public static AcceptedState<TTransition, TState> LinkFixedAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        AcceptedState<TTransition, TState> acceptedState,
        ReduceValue<TTransition, TState> reduce)
        where TTransition : notnull
    {
        var state = acceptedState.State;
        current.LinkFixedState(transition, state, reduce);
        
        return acceptedState;
    }
    
    private static ReduceValue<TTransition, TState> Constant<TTransition, TState>(TState newValue)
        where TTransition : notnull =>
        _ => newValue;

    private static IState<TTransition, TState> AsImmutable<TTransition, TState>(this State<TTransition, TState> current)
        where TTransition : notnull
        => current;
}