namespace DFAutomaton;

public delegate TState Reducer<TTransition, TState>(
    AutomatonRunState<TTransition, TState> runState,
    TState state)
    where TTransition : notnull;