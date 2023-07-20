using Optional;
using Optional.Collections;

namespace DFAutomaton;

public class State<TTransition, TState> : IState<TTransition, TState> where TTransition : notnull
{
    private readonly Dictionary<TTransition, State<TTransition, TState>.Move> _transitionMoveDict = new();

    internal State(StateType type, Func<long> getNextId)
    {
        Id = getNextId();
        Type = type;
        GetNextId = getNextId;
    }

    public long Id { get; }

    public object? Tag { get; set; }

    public StateType Type { get; }

    public IReadOnlySet<TTransition> Transitions => new HashSet<TTransition>(_transitionMoveDict.Keys);

    public Option<State<TTransition, TState>.Move> this[TTransition transition] => _transitionMoveDict.GetValueOrNone(transition);

    Option<IState<TTransition, TState>.Move> IState<TTransition, TState>.this[TTransition transition] =>
        _transitionMoveDict.GetValueOrNone(transition)
            .Map<IState<TTransition, TState>.Move>(next => new(next.NextState, next.Reducer));

    internal Func<long> GetNextId { get; }

    public State<TTransition, TState> LinkState(TTransition transition, State<TTransition, TState> nextState, Reducer<TTransition, TState> reducer)
    {
        if (Type == StateType.Accepted)
            throw new InvalidOperationException("Cannot link a state to the accepted state.");

        var (state, _) = _transitionMoveDict[transition] = new(nextState, reducer);

        return state;
    }

    internal IReadOnlyDictionary<TTransition, State<TTransition, TState>.Move> GetTransitions() => _transitionMoveDict;

    public override string? ToString() => ((IState<TTransition, TState>)this).Format();

    public readonly record struct Move(
        State<TTransition, TState> NextState,
        Reducer<TTransition, TState> Reducer
    );
}