namespace DFAutomaton;

public delegate TState StateReducer<TTransition, TState>(
    AutomatonRunState<TTransition, TState> runState,
    TState state)
    where TTransition : notnull;