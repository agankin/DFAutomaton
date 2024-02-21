using Optional;

namespace DFAutomaton;

/// <summary>
/// This interface provides only readonly state members.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public interface IState<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// Contains an identifier that is unique within the scope of the containing state graph.
    /// </summary>
    long Id { get; }
    
    /// <summary>
    /// Contains a tag with additional information.
    /// </summary>
    object? Tag { get; }

    /// <summary>
    /// Contains the state type.
    /// </summary>
    public StateType Type { get; }

    /// <summary>
    /// Contains transitions to next states.
    /// </summary>
    public IReadOnlyCollection<TTransition> Transitions { get; }

    /// <summary>
    /// Returns a state transition by a transition value.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    /// <returns>Some state transition for the provided transition value or None.</returns>
    Option<Transition<TTransition, TState>> this[TTransition transition] { get; }
    
    /// <summary>
    /// Returns a text representation of the state.
    /// </summary>
    public string Format() => string.IsNullOrEmpty(Tag?.ToString())
        ? $"State {Id}"
        : $"State {Id}: {Tag}";
}