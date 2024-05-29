using PureMonads;

namespace DFAutomaton;

internal class FrozenStateTransitionMap<TTransition, TState> where TTransition : notnull
{
    private IReadOnlyDictionary<TransitionKey<TTransition>, TransitionEntry<TTransition, TState>> _transitionByKey;
    private IReadOnlyDictionary<StateId, TransitionEntry<TTransition, TState>> _fallbackTransitionByStateId;
    private IReadOnlyDictionary<StateId, List<TTransition>> _transitionsByStateId;

    public FrozenStateTransitionMap(
        IReadOnlyDictionary<TransitionKey<TTransition>, TransitionEntry<TTransition, TState>> transitionByKey,
        IReadOnlyDictionary<StateId, TransitionEntry<TTransition, TState>> fallbackTransitionByStateId,
        IReadOnlyDictionary<StateId, List<TTransition>> transitionsByStateId
    )
    {
        _transitionByKey = transitionByKey;
        _fallbackTransitionByStateId = fallbackTransitionByStateId;
        _transitionsByStateId = transitionsByStateId;
    }
    
    public Option<TransitionEntry<TTransition, TState>> this[StateId fromStateId, TTransition transition]
    {
        get
        {
            if (_transitionByKey.TryGetValue(new(fromStateId, transition), out var stateTransition))
                return stateTransition;

            return _fallbackTransitionByStateId.GetOrNone(fromStateId);
        }
    }

    public IReadOnlyCollection<TTransition> GetTransitions(StateId fromStateId) =>
         _transitionsByStateId.GetOr(fromStateId, () => new List<TTransition>());
}