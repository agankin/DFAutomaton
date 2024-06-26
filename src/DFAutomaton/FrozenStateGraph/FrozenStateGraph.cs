using PureMonads;

namespace DFAutomaton;

internal class FrozenStateGraph<TTransition, TState> where TTransition : notnull
{
    private readonly FrozenStateTransitionMap<TTransition, TState> _stateTransitionMap;
    private readonly FrozenStateTagMap _stateTagMap;

    public FrozenStateGraph(StateTransitionMap<TTransition, TState> stateTransitionMap, StateTagMap stateTagMap)
    {
        _stateTransitionMap = stateTransitionMap.ToFrozen(this);
        _stateTagMap = stateTagMap.ToFrozen();
    }

    public FrozenState<TTransition, TState> this[StateId stateId] => new FrozenState<TTransition, TState>(stateId, this);

    public IReadOnlyCollection<FrozenStateTransition<TTransition, TState>> GetTransitions(StateId fromStateId) =>
        _stateTransitionMap.GetTransitions(fromStateId);

    public Option<FrozenTransition<TTransition, TState>> GetTransition(StateId fromStateId, TTransition transition)
    {
        var transitionEntry = _stateTransitionMap[fromStateId, transition];

        return transitionEntry.Map(entry => new FrozenTransition<TTransition, TState>(
            entry.ToStateId.Map(id => this[id]),
            entry.Reducer));
    }

    public object? GetTag(StateId stateId) => _stateTagMap[stateId];

    internal Option<TransitionEntry<TTransition, TState>> GetTransitionEntry(StateId fromStateId, TTransition transition) =>
        _stateTransitionMap[fromStateId, transition];
}