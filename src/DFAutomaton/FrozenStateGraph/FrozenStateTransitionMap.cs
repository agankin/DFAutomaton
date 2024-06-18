using System.Collections.ObjectModel;
using PureMonads;

namespace DFAutomaton;

internal class FrozenStateTransitionMap<TTransition, TState> where TTransition : notnull
{
    private IReadOnlyDictionary<TransitionKey<TTransition>, TransitionEntry<TTransition, TState>> _transitionByKey;
    private readonly ILookup<StateId, TransitionPredicate<TTransition, TState>> _transitionPredicatesByStateId;
    private IReadOnlyDictionary<StateId, TransitionEntry<TTransition, TState>> _fallbackTransitionByStateId;
    private ILookup<StateId, TTransition> _transitionsByStateId;

    public FrozenStateTransitionMap(
        IReadOnlyDictionary<TransitionKey<TTransition>, TransitionEntry<TTransition, TState>> transitionByKey,
        ILookup<StateId, TransitionPredicate<TTransition, TState>> transitionPredicatesByStateId,
        IReadOnlyDictionary<StateId, TransitionEntry<TTransition, TState>> fallbackTransitionByStateId,
        ILookup<StateId, TTransition> transitionsByStateId
    )
    {
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

    public IReadOnlyCollection<TTransition> GetTransitions(StateId fromStateId) => _transitionsByStateId[fromStateId].ToList();
}