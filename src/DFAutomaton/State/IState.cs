using Optional;

namespace DFAutomaton;

public interface IState<TTransition, TState> where TTransition : notnull
{
    long Id { get; }

    object? Tag { get; }

    public StateType Type { get; }

    public IReadOnlySet<TTransition> Transitions { get; }

    Option<Transition<TTransition, TState, IState<TTransition, TState>>> this[TTransition transition] { get; }

    public string Format() => string.IsNullOrEmpty(Tag?.ToString())
        ? $"State {Id}"
        : $"State {Id}: {Tag}";
}