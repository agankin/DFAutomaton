namespace DFAutomaton
{
    public readonly record struct AutomataNextState<TTransition, TState>(
        AutomataState<TTransition, TState> State,
        StateReducer<TTransition, TState> Reducer)
        where TTransition : notnull;
}