namespace DFAutomaton;

internal readonly record struct TransitionKeyEqualityComparer<TTransition> : IEqualityComparer<TransitionKey<TTransition>>
    where TTransition : notnull 
{
    private readonly IEqualityComparer<TTransition> _transitionEqualityComparer;

    public TransitionKeyEqualityComparer(IEqualityComparer<TTransition> transitionEqualityComparer)
    {
        _transitionEqualityComparer = transitionEqualityComparer;
    }

    public bool Equals(TransitionKey<TTransition> first, TransitionKey<TTransition> second)
    {
        if(first.FromStateId != second.FromStateId)
        {
            return false;
        }

        return _transitionEqualityComparer.Equals(first.Transition, second.Transition);
    }

    public int GetHashCode(TransitionKey<TTransition> transitionKey) => transitionKey.GetHashCode();
}