using Optional;

namespace DFAutomaton;

/// <summary>
/// Contains information about transition to a next state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <typeparam name="TDFAState">Automaton state type.</typeparam>
/// <param name="Kind">Transition kind: fixed or dynamic.</param>
/// <param name="State">Some next state for fixed transitions or None for dynamic transitions.</param>
/// <param name="Reduce">Automaton transition reducer.</param>
public record Transition<TTransition, TState, TDFAState>(
    TransitionKind Kind,
    Option<TDFAState> State,
    Reduce<TTransition, TState> Reduce
)
where TTransition : notnull
where TDFAState : IState<TTransition, TState>;

/// <summary>
/// Contains kinds of transitions.
/// </summary>
public enum TransitionKind
{
    /// <summary>
    /// A transition to a fixed state.
    /// </summary>
    FixedState = 1,

    /// <summary>
    /// A dynamic transition.
    /// </summary>
    DynamicGoTo
}