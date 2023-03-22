namespace DFAutomaton
{
    public delegate TState StateReducer<TTransition, TState>(
        AutomataControl<TTransition> control,
        TState state);
}