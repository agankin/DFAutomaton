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
        TState newStateValue)
        where TTransition : notnull
    {
        var newStateReducer = ConstantReducer<TTransition, TState>(newStateValue);
        return current.ToNewState(transition, newStateReducer);
    }

    public static State<TTransition, TState> ToNewState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        Reducer<TTransition, TState> reducer)
        where TTransition : notnull
    {
        var newState = StateFactory<TTransition, TState>.SubState(current.GetNextId);
        return current.LinkState(transition, newState, reducer);
    }

    public static State<TTransition, TState> LinkState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        State<TTransition, TState> nextState,
        TState nextStateValue)
        where TTransition : notnull
    {
        var reducer = ConstantReducer<TTransition, TState>(nextStateValue);
        return current.LinkState(transition, nextState, reducer);
    }

    public static AcceptedState<TTransition, TState> ToNewAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        TState acceptedStateValue)
        where TTransition : notnull
    {
        var reducer = ConstantReducer<TTransition, TState>(acceptedStateValue);
        return current.ToNewAccepted(transition, reducer);
    }

    public static AcceptedState<TTransition, TState> ToNewAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        Reducer<TTransition, TState> reducer)
        where TTransition : notnull
    {
        var acceptedState = StateFactory<TTransition, TState>.Accepted(current.GetNextId);
        current.LinkState(transition, acceptedState, reducer);

        return new AcceptedState<TTransition, TState>(acceptedState);
    }

    public static AcceptedState<TTransition, TState> LinkAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        AcceptedState<TTransition, TState> acceptedState,
        TState acceptedStateValue)
        where TTransition : notnull
    {
        var reducer = ConstantReducer<TTransition, TState>(acceptedStateValue);
        return current.LinkAccepted(transition, acceptedState, reducer);
    }

    public static AcceptedState<TTransition, TState> LinkAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        AcceptedState<TTransition, TState> acceptedState,
        Reducer<TTransition, TState> reducer)
        where TTransition : notnull
    {
        var state = acceptedState.State;
        current.LinkState(transition, state, reducer);
        
        return acceptedState;
    }

    private static Reducer<TTransition, TState> ConstantReducer<TTransition, TState>(TState newStateValue) where TTransition : notnull =>
        (_, _) => newStateValue;

    private static IState<TTransition, TState> AsImmutable<TTransition, TState>(this State<TTransition, TState> current)
        where TTransition : notnull
        => current;
}