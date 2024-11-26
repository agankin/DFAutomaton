using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Contains information about an automaton runtime error.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="Type">The error type.</param>
/// <param name="WhenTransitioningFrom">
/// Option containing a state the error occured when transitioning from.
/// </param>
/// <param name="Transition">A transition caused the error.</param>
/// <param name="StateValue">The current state value.</param>
public record AutomatonError<TTransition, TState>(
    AutomatonErrorType Type,
    Option<FrozenState<TTransition, TState>> WhenTransitioningFrom,
    Option<TTransition> Transition,
    TState StateValue
)
where TTransition : notnull;