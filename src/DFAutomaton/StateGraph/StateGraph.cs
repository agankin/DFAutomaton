using PureMonads;

namespace DFAutomaton;

internal class StateGraph<TTransition, TState> where TTransition : notnull
{
    private readonly StateTransitionMap<TTransition, TState> _stateTransitionMap = new();
    private readonly StateTagMap _stateTagMap = new();
    private StateId _nextId = StateId.SubStateStartId;
    
    public State<TTransition, TState> this[StateId stateId] => new State<TTransition, TState>(stateId, this);

    public State<TTransition, TState> StartState => this[StateId.StartStateId];

    public State<TTransition, TState> AcceptedState => this[StateId.AcceptedStateId];

    public State<TTransition, TState> CreateState() => new State<TTransition, TState>(_nextId++, this);

    public IReadOnlyCollection<TTransition> GetTransitions(StateId fromStateId) =>
        _stateTransitionMap.GetTransitions(fromStateId);

    public Option<Transition<TTransition, TState>> GetStateTransition(StateId fromStateId, TTransition transition)
    {
        var transitionEntry = _stateTransitionMap[fromStateId, transition];

        return transitionEntry.Map(entry => new Transition<TTransition, TState>(
            entry.ToStateId.Map(id => this[id]),
            entry.Reducer));
    }

    public void AddStateTransition(StateId fromStateId, TTransition transition, Transition<TTransition, TState> stateTransition) =>
        _stateTransitionMap.AddStateTransition(fromStateId, transition, stateTransition);

    public void AddFallbackTransition(StateId fromStateId, Transition<TTransition, TState> stateTransition) =>
        _stateTransitionMap.AddFallbackTransition(fromStateId, stateTransition);

    public object? GetTag(StateId stateId) => _stateTagMap[stateId];

    public void SetTag(StateId stateId, object? value) => _stateTagMap[stateId] = value;

    public FrozenStateGraph<TTransition, TState> ToFrozen()
    {
        var stateTransitionMap = _stateTransitionMap.ToFrozen();
        var stateTagMap = _stateTagMap.ToFrozen();

        return new FrozenStateGraph<TTransition, TState>(stateTransitionMap, stateTagMap);
    }
}