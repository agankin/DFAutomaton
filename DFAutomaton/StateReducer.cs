namespace DFAutomaton
{
    public readonly record struct StateReducer<TTransition, TState>(
        State<TTransition, TState> State,
        Func<TState, TState> Reducer)
        where TTransition : notnull;
}