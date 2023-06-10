using Optional;
using Optional.Collections;

namespace DFAutomaton;

internal class AutomatonState<TTransition, TState> : IState<TTransition, TState> where TTransition : notnull
{
    private readonly IStateTransitionDict<TTransition, TState> _transitions;

    internal AutomatonState(long id, object? tag, StateType type, IStateTransitionDict<TTransition, TState> transitions)
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

    public Option<StateTransition<TTransition, TState>> this[TTransition transition] => _transitions.GetValueOrNone(transition);

    public override string? ToString() => this.Format();
}