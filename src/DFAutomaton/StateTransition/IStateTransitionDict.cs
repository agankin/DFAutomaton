namespace DFAutomaton;

internal interface IStateTransitionDict<TTransition, TState> : IReadOnlyDictionary<TTransition, StateTransition<TTransition, TState>>
    where TTransition : notnull
{
}