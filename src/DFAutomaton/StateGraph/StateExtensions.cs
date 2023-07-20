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

    public static State<TTransition, TState> ToNewState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        TState newValue)
        where TTransition : notnull
    {
        var reduce = Constant<TTransition, TState>(newValue);
        return current.ToNewState(transition, reduce);
    }

    public static State<TTransition, TState> ToNewState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        Reduce<TState> reduce)
        where TTransition : notnull
    {
        var newState = StateFactory<TTransition, TState>.SubState(current.GetNextId);
        return current.LinkState(transition, newState, reduce);
    }

    public static State<TTransition, TState> LinkState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        State<TTransition, TState> nextState,
        TState nextValue)
        where TTransition : notnull
    {
        var reduce = Constant<TTransition, TState>(nextValue);
        return current.LinkState(transition, nextState, reduce);
    }

    public static AcceptedState<TTransition, TState> ToNewAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        TState acceptedValue)
        where TTransition : notnull
    {
        var reduce = Constant<TTransition, TState>(acceptedValue);
        return current.ToNewAccepted(transition, reduce);
    }

    public static AcceptedState<TTransition, TState> ToNewAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        Reduce<TState> reduce)
        where TTransition : notnull
    {
        var acceptedState = StateFactory<TTransition, TState>.Accepted(current.GetNextId);
        current.LinkState(transition, acceptedState, reduce);

        return new AcceptedState<TTransition, TState>(acceptedState);
    }

    public static AcceptedState<TTransition, TState> LinkAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        AcceptedState<TTransition, TState> acceptedState,
        TState acceptedStateValue)
        where TTransition : notnull
    {
        var reduce = Constant<TTransition, TState>(acceptedStateValue);
        return current.LinkAccepted(transition, acceptedState, reduce);
    }

    public static AcceptedState<TTransition, TState> LinkAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        AcceptedState<TTransition, TState> acceptedState,
        Reduce<TState> reduce)
        where TTransition : notnull
    {
        var state = acceptedState.State;
        current.LinkState(transition, state, reduce);
        
        return acceptedState;
    }

    private static Reduce<TState> Constant<TTransition, TState>(TState newValue) where TTransition : notnull =>
        _ => newValue;

    private static IState<TTransition, TState> AsImmutable<TTransition, TState>(this State<TTransition, TState> current)
        where TTransition : notnull
        => current;
}