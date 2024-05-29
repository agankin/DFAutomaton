using PureMonads;

namespace DFAutomaton;

internal class FrozenStateGraph<TTransition, TState> where TTransition : notnull
{
    private readonly FrozenStateTransitionMap<TTransition, TState> _stateTransitionMap;
    private readonly FrozenStateTagMap _stateTagMap;

    public FrozenStateGraph(
        FrozenStateTransitionMap<TTransition, TState> stateTransitionMap,
        FrozenStateTagMap stateTagMap)
    {
        _stateTransitionMap = stateTransitionMap;
        _stateTagMap = stateTagMap;
    }

    public FrozenState<TTransition, TState> this[StateId stateId] => new FrozenState<TTransition, TState>(stateId, this);

    public IReadOnlyCollection<TTransition> GetTransitions(StateId fromStateId) =>
        _stateTransitionMap.GetTransitions(fromStateId);

    public Option<FrozenTransition<TTransition, TState>> GetStateTransition(StateId fromStateId, TTransition transition)
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