namespace DFAutomaton
{
    public delegate TState StateReducer<TTransition, TState>(
        AutomataRunState<TTransition, TState> runState,
        TState state)
        where TTransition : notnull;
}