namespace DFAutomaton;

public readonly record struct StateTransition<TTransition, TState>(
    IState<TTransition, TState> NextState,
    StateReducer<TTransition, TState> Reducer
) where TTransition : notnull;