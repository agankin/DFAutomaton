using Optional;

namespace DFAutomaton;

/// <summary>
/// Contains information about an automaton runtime error.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="Type">The error type.</param>
/// <param name="WhenTransitioningFrom">
/// Some automaton state the error occured when transitioning from.
/// </param>
/// <param name="Transition">A transition caused the error.</param>
/// <param name="ErrorState">
/// Some state returned from a reducer and determined to be a error state.
/// Set when type is <see cref="AutomatonErrorType.ReducerError"/>.
/// </param>
public record AutomatonError<TTransition, TState>(
    AutomatonErrorType Type,
    Option<ImmutableState<TTransition, TState>> WhenTransitioningFrom,
    Option<TTransition> Transition,
    Option<TState> ErrorState
)
where TTransition : notnull;