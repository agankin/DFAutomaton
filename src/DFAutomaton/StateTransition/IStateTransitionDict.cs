namespace DFAutomaton;

internal interface ITransitionDict<TTransition, TState, TDFAState> : IReadOnlyDictionary<TTransition, Transition<TTransition, TState, TDFAState>>
    where TTransition : notnull
    where TDFAState : IState<TTransition, TState>
{
}