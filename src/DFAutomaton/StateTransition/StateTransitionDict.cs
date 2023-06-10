namespace DFAutomaton;

internal class StateTransitionDict<TTransition, TState>
    : Dictionary<TTransition, StateTransition<TTransition, TState>>, IStateTransitionDict<TTransition, TState>
    where TTransition : notnull
{
    public StateTransitionDict() : base() {}

    public StateTransitionDict(IDictionary<TTransition, StateTransition<TTransition, TState>> dictionary) : base(dictionary)
    {
    }
}