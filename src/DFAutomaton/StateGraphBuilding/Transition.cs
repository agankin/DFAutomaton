namespace DFAutomaton;

public readonly record struct Transition<TTransition, TState>(
    State<TTransition, TState> NextState,
    StateReducer<TTransition, TState> Reducer)
    where TTransition : notnull;