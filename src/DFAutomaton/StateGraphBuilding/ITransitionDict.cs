namespace DFAutomaton;

internal interface ITransitionDict<TTransition, TState> : IReadOnlyDictionary<TTransition, Transition<TTransition, TState>>
    where TTransition : notnull 
{
}