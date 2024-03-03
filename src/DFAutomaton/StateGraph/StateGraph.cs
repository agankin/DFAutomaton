using Optional;

namespace DFAutomaton;

internal class StateGraph<TTransition, TState> where TTransition : notnull
{
    private readonly StateTransitionMap<TTransition, TState> _stateTransitionMap = new();
    private readonly StateTagMap _stateTagMap = new();

    private uint _nextId = StateId.SubStateStartId;

    public State<TTransition, TState> this[uint stateId] => new State<TTransition, TState>(stateId, this);

    public State<TTransition, TState> StartState => this[StateId.StartStateId];

    public State<TTransition, TState> CreateState() => new State<TTransition, TState>(_nextId++, this);

    public IReadOnlyCollection<TTransition> GetTransitions(uint fromStateId) =>
        _stateTransitionMap.GetTransitions(fromStateId);

    public Option<Transition<TTransition, TState>> GetStateTransition(uint fromStateId, TTransition transition) =>
        _stateTransitionMap[fromStateId, transition];

    public void AddStateTransition(uint fromStateId, TTransition transition, Transition<TTransition, TState> stateTransition) =>
        _stateTransitionMap.AddStateTransition(fromStateId, transition, stateTransition);

    public object? GetTag(uint stateId) => _stateTagMap[stateId];

    public void SetTag(uint stateId, object? value) => _stateTagMap[stateId] = value;
}