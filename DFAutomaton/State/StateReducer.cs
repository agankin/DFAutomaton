namespace DFAutomaton
{
    public delegate TState StateReducer<TTransition, TState>(
        AutomataRunState<TTransition> control,
        TState state);
}