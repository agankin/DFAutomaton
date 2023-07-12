using Optional;
using Optional.Collections;

namespace DFAutomaton;

internal class AutomatonState<TTransition, TState> : IState<TTransition, TState> where TTransition : notnull
{
    private readonly ITransitionDict<TTransition, TState, IState<TTransition, TState>> _transitions;

    internal AutomatonState(long id, object? tag, StateType type, ITransitionDict<TTransition, TState, IState<TTransition, TState>> transitions)
    {
        Id = id;
        Tag = tag;
        Type = type;
        _transitions = transitions;
    }

    public long Id { get; }

    public object? Tag { get; }

    public StateType Type { get; }

    public IReadOnlySet<TTransition> Transitions => new HashSet<TTransition>(_transitions.Keys);

    public Option<Transition<TTransition, TState, IState<TTransition, TState>>> this[TTransition transition] => _transitions.GetValueOrNone(transition);

    public override string? ToString() => ((IState<TTransition, TState>)this).Format();
}