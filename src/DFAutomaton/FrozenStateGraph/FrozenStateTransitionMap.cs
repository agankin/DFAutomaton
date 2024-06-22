using PureMonads;

namespace DFAutomaton;

internal class FrozenStateTransitionMap<TTransition, TState> where TTransition : notnull
{
    private readonly FrozenStateGraph<TTransition, TState> _owningGraph;

    private IReadOnlyDictionary<TransitionKey<TTransition>, TransitionEntry<TTransition, TState>> _transitionByKey;
    private readonly ILookup<StateId, TransitionPredicate<TTransition, TState>> _transitionPredicatesByStateId;
    private IReadOnlyDictionary<StateId, TransitionEntry<TTransition, TState>> _fallbackTransitionByStateId;
    private ILookup<StateId, TTransition> _transitionsByStateId;

    public FrozenStateTransitionMap(
        FrozenStateGraph<TTransition, TState> owningGraph,
        IReadOnlyDictionary<TransitionKey<TTransition>, TransitionEntry<TTransition, TState>> transitionByKey,
        ILookup<StateId, TransitionPredicate<TTransition, TState>> transitionPredicatesByStateId,
        IReadOnlyDictionary<StateId, TransitionEntry<TTransition, TState>> fallbackTransitionByStateId,
        ILookup<StateId, TTransition> transitionsByStateId
    )
    {
        _owningGraph = owningGraph;
        _transitionByKey = transitionByKey;
        _transitionPredicatesByStateId = transitionPredicatesByStateId;
        _fallbackTransitionByStateId = fallbackTransitionByStateId;
        _transitionsByStateId = transitionsByStateId;
    }
    
    public Option<TransitionEntry<TTransition, TState>> this[StateId fromStateId, TTransition transition]
    {
        get
        {
            if (_transitionByKey.TryGetValue(new(fromStateId, transition), out var transitionEntry))
                return transitionEntry;

            if (_transitionPredicatesByStateId.TryFindTransition(fromStateId, transition, out transitionEntry))
                return transitionEntry;

            return _fallbackTransitionByStateId.GetOrNone(fromStateId);
        }
    }

    public IReadOnlyCollection<FrozenStateTransition<TTransition, TState>> GetTransitions(StateId fromStateId)
    {
        var transitions = _transitionsByStateId[fromStateId];
        var stateTransitionsForValues = transitions.Select(value => GetStateTransition(value, fromStateId));

        var transitionPredicates = _transitionPredicatesByStateId[fromStateId];
        var stateTransitionsForPredicates = transitionPredicates.Select(GetStateTransition);

        return stateTransitionsForValues.Concat(stateTransitionsForPredicates).ToList();
    }

    private FrozenStateTransition<TTransition, TState> GetStateTransition(TTransition transitionValue, StateId fromStateId)
    {
        var (toStateId, reducer) = _transitionByKey[new(fromStateId, transitionValue)];
        var toState = toStateId.Map(id => new FrozenState<TTransition, TState>(id, _owningGraph));
        var transition = new FrozenTransition<TTransition, TState>(toState, reducer);

        return new FrozenStateTransition<TTransition, TState>(transitionValue, transition);
    }

    private FrozenStateTransition<TTransition, TState> GetStateTransition(TransitionPredicate<TTransition, TState> transitionPredicate)
    {
        var (predicateName, predicate, transitionEntry) = transitionPredicate;
        var (toStateId, reducer) = transitionEntry;

        var canTransit = new CanTransit<TTransition>(predicateName, predicate);
        var toState = toStateId.Map(id => new FrozenState<TTransition, TState>(id, _owningGraph));
        var transition = new FrozenTransition<TTransition, TState>(toState, reducer);

        return new FrozenStateTransition<TTransition, TState>(canTransit, transition);
    }
}