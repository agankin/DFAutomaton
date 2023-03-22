namespace DFAutomaton
{
    public readonly record struct Next<TTransition, TState>(
        IState<TTransition, TState> State,
        StateReducer<TTransition, TState> Reducer)
        where TTransition : notnull;
}