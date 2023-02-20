namespace DFAutomaton
{
    public readonly record struct AutomataNextState<TTransition, TState>(
        AutomataState<TTransition, TState> State,
        Func<TState, TState> Reducer)
        where TTransition : notnull;
}