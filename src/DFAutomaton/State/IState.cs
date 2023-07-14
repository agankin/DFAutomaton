using Optional;

namespace DFAutomaton;

public interface IState<TTransition, TState> where TTransition : notnull
{
    long Id { get; }

    object? Tag { get; }

    public StateType Type { get; }

    public IReadOnlySet<TTransition> Transitions { get; }

    Option<IState<TTransition, TState>.Move> this[TTransition transition] { get; }

    public string Format() => string.IsNullOrEmpty(Tag?.ToString())
        ? $"State {Id}"
        : $"State {Id}: {Tag}";

    public readonly record struct Move(
        IState<TTransition, TState> NextState,
        Reducer<TTransition, TState> Reducer
    );
}