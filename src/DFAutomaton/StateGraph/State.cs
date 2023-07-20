using Optional;
using Optional.Collections;

namespace DFAutomaton;

public class State<TTransition, TState> : IState<TTransition, TState> where TTransition : notnull
{
    private readonly Dictionary<TTransition, State<TTransition, TState>.Transition> _transitionDict = new();

    internal State(StateType type, Func<long> getNextId)
    {
        Id = getNextId();
        Type = type;
        GetNextId = getNextId;
    }

    public long Id { get; }

    public object? Tag { get; set; }

    public StateType Type { get; }

    public IReadOnlySet<TTransition> Transitions => new HashSet<TTransition>(_transitionDict.Keys);

    public Option<Transition> this[TTransition transition] => _transitionDict.GetValueOrNone(transition);

    Option<IState<TTransition, TState>.Transition> IState<TTransition, TState>.this[TTransition transition] =>
        _transitionDict.GetValueOrNone(transition).Map<IState<TTransition, TState>.Transition>(transition =>
            new(transition.State.Map<IState<TTransition, TState>>(_ => _), transition.GoToState, transition.Reduce));

    internal Func<long> GetNextId { get; }

    public State<TTransition, TState> LinkState(TTransition transition, State<TTransition, TState> nextState, Reduce<TState> reduce)
    {
        if (Type == StateType.Accepted)
            throw new InvalidOperationException("Cannot link a state to the accepted state.");

        _transitionDict[transition] = new(nextState.Some(), _ => nextState, reduce);

        return nextState;
    }

    public void GoToState(
        TTransition transition,
        GoToState<TTransition, TState, State<TTransition, TState>> goToState,
        Reduce<TState> reduce)
    {
        if (Type == StateType.Accepted)
            throw new InvalidOperationException("Cannot link a state to the accepted state.");

        _transitionDict[transition] = new(Option.None<State<TTransition, TState>>(), goToState, reduce);
    }

    internal IReadOnlyDictionary<TTransition, Transition> GetTransitions() => _transitionDict;

    public override string? ToString() => ((IState<TTransition, TState>)this).Format();

    public record Transition(
        Option<State<TTransition, TState>> State,
        GoToState<TTransition, TState, State<TTransition, TState>> GoToState,
        Reduce<TState> Reduce
    ) : Transition<TTransition, TState, State<TTransition, TState>>(State, GoToState, Reduce);
}