namespace DFAutomaton;

internal record TransitionKeyEqualityComparer<TTransition> : IEqualityComparer<TransitionKey<TTransition>>
    where TTransition : notnull 
{
    private readonly IEqualityComparer<TTransition> _transitionEqualityComparer;

    public TransitionKeyEqualityComparer(IEqualityComparer<TTransition> transitionEqualityComparer)
    {
        _transitionEqualityComparer = transitionEqualityComparer;
    }

    public bool Equals(TransitionKey<TTransition> first, TransitionKey<TTransition> second)
    {
        if (first.FromStateId != second.FromStateId)
        {
            return false;
        }

        return _transitionEqualityComparer.Equals(first.Transition, second.Transition);
    }

    public int GetHashCode(TransitionKey<TTransition> transitionKey)
    {
        var (fromStateId, transition) = transitionKey;

        return ((int)fromStateId.Value << 16) + _transitionEqualityComparer.GetHashCode(transition);
    }
}