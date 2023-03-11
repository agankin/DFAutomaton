namespace DFAutomaton
{
    public readonly record struct NextState<TTransition, TState>(
        State<TTransition, TState> State,
        StateReducer<TTransition, TState> Reducer)
        where TTransition : notnull;
}