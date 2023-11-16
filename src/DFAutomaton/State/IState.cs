using Optional;

namespace DFAutomaton;

/// <summary>
/// Automaton state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public interface IState<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// State unique id.
    /// </summary>
    long Id { get; }
    
    /// <summary>
    /// Tag with some additional information attached to the state.
    /// </summary>
    object? Tag { get; }

    /// <summary>
    /// State type.
    /// </summary>
    public StateType Type { get; }

    /// <summary>
    /// Transition values to next states.
    /// </summary>
    public IReadOnlyCollection<TTransition> Transitions { get; }

    /// <summary>
    /// Get state transition by transition value.
    /// </summary>
    /// <param name="transition">Transition value.</param>
    /// <returns>State transition.</returns>
    Option<IState<TTransition, TState>.Transition> this[TTransition transition] { get; }
    
    /// <summary>
    /// Returns text representation of the state.
    /// </summary>
    public string Format() => string.IsNullOrEmpty(Tag?.ToString())
        ? $"State {Id}"
        : $"State {Id}: {Tag}";

    /// <summary>
    /// State transition.
    /// </summary>
    /// <param name="Kind">Transition kind.</param>
    /// <param name="State">Some next state for fixed transitions or None for dynamic transitions.</param>
    /// <param name="Reduce">State reducer.</param>
    public record Transition(
        TransitionKind Kind,
        Option<IState<TTransition, TState>> State,
        Reduce<TTransition, TState> Reduce
    ) : Transition<TTransition, TState, IState<TTransition, TState>>(Kind, State, Reduce);
}