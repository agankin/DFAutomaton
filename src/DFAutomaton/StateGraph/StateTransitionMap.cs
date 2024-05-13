using Optional;

namespace DFAutomaton;

internal class StateTransitionMap<TTransition, TState> where TTransition : notnull
{
    private Dictionary<TransitionKey, Transition<TTransition, TState>> _transitionByKey = new();
    private Dictionary<uint, Transition<TTransition, TState>> _fallbackTransitionByStateId = new();
    private Dictionary<uint, List<TTransition>> _transitionsByStateId = new();

    public Option<Transition<TTransition, TState>> this[uint fromStateId, TTransition transition]
    {
        get
        {
            if (_transitionByKey.TryGetValue(new(fromStateId, transition), out var stateTransition))
                return stateTransition.Some();

            return _fallbackTransitionByStateId.GetOrNone(fromStateId);
        }
    }

    public IReadOnlyCollection<TTransition> GetTransitions(uint fromStateId) =>
         _transitionsByStateId.GetOr(fromStateId, () => new List<TTransition>());

    public void AddStateTransition(uint fromStateId, TTransition transition, Transition<TTransition, TState> stateTransition)
    {
        var key = new TransitionKey(fromStateId, transition);
        _transitionByKey[key] = stateTransition;

        if (!_transitionsByStateId.TryGetValue(fromStateId, out var transitions))
            _transitionsByStateId[fromStateId] = transitions = new List<TTransition>();

        transitions.Add(transition);
    }

    public void AddFallbackTransition(uint fromStateId, Transition<TTransition, TState> stateTransition) =>
        _fallbackTransitionByStateId[fromStateId] = stateTransition;

    private readonly record struct TransitionKey(
        uint FromStateId,
        TTransition Transition
    );
}