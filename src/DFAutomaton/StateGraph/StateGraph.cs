using PureMonads;

namespace DFAutomaton;

internal class StateGraph<TTransition, TState> where TTransition : notnull
{
    private readonly StateTransitionMap<TTransition, TState> _stateTransitionMap;
    private readonly StateTagMap _stateTagMap = new();
    private StateId _nextId = StateId.SubStateStartId;

    public StateGraph()
    {
        _stateTransitionMap = new(this);
    }

    public State<TTransition, TState> this[StateId stateId] => new(stateId, this);

    public State<TTransition, TState> StartState => this[StateId.StartStateId];

    public State<TTransition, TState> AcceptedState => this[StateId.AcceptedStateId];

    public State<TTransition, TState> CreateState() => new(_nextId++, this);

    public IReadOnlyCollection<StateTransition<TTransition, TState>> GetTransitions(StateId fromStateId) =>
        _stateTransitionMap.GetTransitions(fromStateId);

    public Option<Transition<TTransition, TState>> GetTransition(StateId fromStateId, TTransition transition)
    {
        var transitionEntry = _stateTransitionMap[fromStateId, transition];

        return transitionEntry.Map(entry => new Transition<TTransition, TState>(
            entry.ToStateId.Map(id => this[id]),
            entry.Reducer));
    }

    public void AddTransition(
        StateId fromStateId,
        Either<TTransition, Predicate<TTransition>> transitionValueOrPredicate,
        Transition<TTransition, TState> transition)
    {
        transitionValueOrPredicate.Match(
            value => _stateTransitionMap.AddTransition(fromStateId, value, transition),
            predicate => _stateTransitionMap.AddTransition(fromStateId, predicate, transition)
        );
    }

    public void AddFallbackTransition(StateId fromStateId, Transition<TTransition, TState> transition) =>
        _stateTransitionMap.AddFallbackTransition(fromStateId, transition);

    public object? GetTag(StateId stateId) => _stateTagMap[stateId];

    public void SetTag(StateId stateId, object? value) => _stateTagMap[stateId] = value;

    public FrozenStateGraph<TTransition, TState> ToFrozen()
    {
        var stateTransitionMap = _stateTransitionMap.ToFrozen();
        var stateTagMap = _stateTagMap.ToFrozen();

        return new FrozenStateGraph<TTransition, TState>(stateTransitionMap, stateTagMap);
    }
}