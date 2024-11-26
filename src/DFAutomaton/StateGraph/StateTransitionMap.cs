using PureMonads;

namespace DFAutomaton;

internal class StateTransitionMap<TTransition, TState> where TTransition : notnull
{
    private readonly Dictionary<StateId, List<TransitionPredicate<TTransition, TState>>> _transitionPredicatesByStateId = new();
    private readonly Dictionary<StateId, TransitionEntry<TTransition, TState>> _fallbackTransitionByStateId = new();
    private readonly Dictionary<StateId, List<TTransition>> _transitionsByStateId = new();

    private readonly StateGraph<TTransition, TState> _owningGraph;
    private readonly IEqualityComparer<TransitionKey<TTransition>> _transitionKeyEqualityComparer;
    private readonly Dictionary<TransitionKey<TTransition>, TransitionEntry<TTransition, TState>> _transitionByKey;

    public StateTransitionMap(StateGraph<TTransition, TState> owningGraph, IEqualityComparer<TTransition> transitionEqualityComparer)
    {
        _owningGraph = owningGraph;

        _transitionKeyEqualityComparer = new TransitionKeyEqualityComparer<TTransition>(transitionEqualityComparer);
        _transitionByKey = new(_transitionKeyEqualityComparer);
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

    public IReadOnlyCollection<StateTransition<TTransition, TState>> GetTransitions(StateId fromStateId)
    {
        var transitions = _transitionsByStateId.GetOr(fromStateId, () => new List<TTransition>());
        var stateTransitionsForValues = transitions.Select(value => GetStateTransition(value, fromStateId));

        var transitionPredicates = _transitionPredicatesByStateId.GetOr(fromStateId, () => new List<TransitionPredicate<TTransition, TState>>());
        var stateTransitionsForPredicates = transitionPredicates.Select(GetStateTransition);

        return stateTransitionsForValues.Concat(stateTransitionsForPredicates).ToList();
    }

    public void AddTransition(StateId fromStateId, TTransition transitionValue, Transition<TTransition, TState> transition)
    {
        var key = new TransitionKey<TTransition>(fromStateId, transitionValue);
        _transitionByKey[key] = transition.ToEntry();

        _transitionsByStateId.AddValue(fromStateId, transitionValue);
    }

    public void AddTransition(StateId fromStateId, CanTransit<TTransition> canTransit, Transition<TTransition, TState> transition)
    {
        var transitionEntry = transition.ToEntry();
        var (name, predicate) = canTransit;
        var transitionPredicate = new TransitionPredicate<TTransition, TState>(name, predicate, transitionEntry);

        _transitionPredicatesByStateId.AddValue(fromStateId, transitionPredicate);
    }

    public void AddFallbackTransition(StateId fromStateId, Transition<TTransition, TState> transition) =>
        _fallbackTransitionByStateId[fromStateId] = transition.ToEntry();

    public FrozenStateTransitionMap<TTransition, TState> ToFrozen(FrozenStateGraph<TTransition, TState> owningGraph)
    {
        var transitionByKey = _transitionByKey.ToFrozen(_transitionKeyEqualityComparer);
        var transitionPredicatesByStateId = _transitionPredicatesByStateId.ToFrozen();
        var fallbackTransitionByStateId = _fallbackTransitionByStateId.ToFrozen();
        var transitionsByStateId = _transitionsByStateId.ToFrozen();
        
        return new FrozenStateTransitionMap<TTransition, TState>(
            owningGraph,
            transitionByKey,
            transitionPredicatesByStateId,
            fallbackTransitionByStateId,
            transitionsByStateId);
    }

    private StateTransition<TTransition, TState> GetStateTransition(TTransition transitionValue, StateId fromStateId)
    {
        var (toStateId, reducer) = _transitionByKey[new(fromStateId, transitionValue)];
        var toState = toStateId.Map(id => new State<TTransition, TState>(id, _owningGraph));
        var transition = new Transition<TTransition, TState>(toState, reducer);

        return new StateTransition<TTransition, TState>(transitionValue, transition);
    }

    private StateTransition<TTransition, TState> GetStateTransition(TransitionPredicate<TTransition, TState> transitionPredicate)
    {
        var (predicateName, predicate, transitionEntry) = transitionPredicate;
        var (toStateId, reducer) = transitionEntry;

        var canTransit = new CanTransit<TTransition>(predicateName, predicate);
        var toState = toStateId.Map(id => new State<TTransition, TState>(id, _owningGraph));
        var transition = new Transition<TTransition, TState>(toState, reducer);

        return new StateTransition<TTransition, TState>(canTransit, transition);
    }
}