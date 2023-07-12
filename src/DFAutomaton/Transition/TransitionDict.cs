namespace DFAutomaton;

internal class TransitionDict<TTransition, TState, TDFAState>
    : Dictionary<TTransition, Transition<TTransition, TState, TDFAState>>, ITransitionDict<TTransition, TState, TDFAState>
    where TTransition : notnull
    where TDFAState : IState<TTransition, TState>
{
    public TransitionDict() : base() {}

    public TransitionDict(IDictionary<TTransition, Transition<TTransition, TState, TDFAState>> dictionary) : base(dictionary)
    {
    }
}