using Optional;

namespace DFAutomaton;

/// <summary>
/// State transition.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <typeparam name="TDFAState">Automaton state type.</typeparam>
/// <param name="Type">Transition kind.</param>
/// <param name="State">Next automaton state if fixed transition or none if dynamic.</param>
/// <param name="Reduce">Automaton state reducer.</param>
public record Transition<TTransition, TState, TDFAState>(
    TransitionType Type,
    Option<TDFAState> State,
    Reduce<TTransition, TState> Reduce
)
where TTransition : notnull
where TDFAState : IState<TTransition, TState>;

/// <summary>
/// Transition kind.
/// </summary>
public enum TransitionType
{
    /// <summary>
    /// Transition to fixed state.
    /// </summary>
    FixedState = 1,

    /// <summary>
    /// Dynamic transition.
    /// </summary>
    DynamicGoTo
}