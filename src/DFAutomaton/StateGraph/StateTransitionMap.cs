using PureMonads;

namespace DFAutomaton;

internal class StateTransitionMap<TTransition, TState> where TTransition : notnull
{
    private Dictionary<TransitionKey<TTransition>, TransitionEntry<TTransition, TState>> _transitionByKey = new();
    private Dictionary<StateId, TransitionEntry<TTransition, TState>> _fallbackTransitionByStateId = new();
    private Dictionary<StateId, List<TTransition>> _transitionsByStateId = new();

    public Option<TransitionEntry<TTransition, TState>> this[StateId fromStateId, TTransition transition]
    {
        get
        {
            if (_transitionByKey.TryGetValue(new(fromStateId, transition), out var transitionEntry))
                return transitionEntry;

            return _fallbackTransitionByStateId.GetOrNone(fromStateId);
        }
    }

    public IReadOnlyCollection<TTransition> GetTransitions(StateId fromStateId) =>
         _transitionsByStateId.GetOr(fromStateId, () => new List<TTransition>());

    public void AddStateTransition(StateId fromStateId, TTransition transition, Transition<TTransition, TState> stateTransition)
    {
        var key = new TransitionKey<TTransition>(fromStateId, transition);
        _transitionByKey[key] = stateTransition.ToEntry();

        if (!_transitionsByStateId.TryGetValue(fromStateId, out var transitions))
            _transitionsByStateId[fromStateId] = transitions = new List<TTransition>();

        transitions.Add(transition);
    }

    public void AddFallbackTransition(StateId fromStateId, Transition<TTransition, TState> stateTransition) =>
        _fallbackTransitionByStateId[fromStateId] = stateTransition.ToEntry();

    public FrozenStateTransitionMap<TTransition, TState> ToFrozen()
    {
        var transitionByKey = _transitionByKey.ToDictionary(e => e.Key, e => e.Value);
        var fallbackTransitionByStateId = _fallbackTransitionByStateId.ToDictionary(e => e.Key, e => e.Value);
        var transitionsByStateId = _transitionsByStateId.ToDictionary(e => e.Key, e => e.Value);
        
        return new FrozenStateTransitionMap<TTransition, TState>(
            transitionByKey,
            fallbackTransitionByStateId,
            transitionsByStateId);
    }
}