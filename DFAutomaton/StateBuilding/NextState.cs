namespace DFAutomaton
{
    public readonly record struct NextState<TTransition, TState>(
        State<TTransition, TState> State,
        Func<TState, TState> Reducer)
        where TTransition : notnull;
}