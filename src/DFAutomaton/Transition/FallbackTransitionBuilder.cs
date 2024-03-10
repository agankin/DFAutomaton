namespace DFAutomaton;

/// <summary>
/// A builder for creating fallback dynamic transition.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="FromState">A state the fallback transition is starting from.</param>
/// <remarks>
/// Fallback transition is a dynamic transition that is invoked for all unknown transition values.
/// </remarks>
public record FallbackTransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState
) where TTransition : notnull
{
    /// <summary>
    /// Adds a new dynamic fallback transition and completes the build.
    /// </summary>
    /// <param name="reducer">An automaton transition reducer.</param>
    /// <remarks>
    /// Fallback transition is a dynamic transition that is invoked for all unknown transition values.
    /// </remarks>
    public void WithReducing(Reduce<TTransition, TState> reducer) => FromState.AddFallbackTransition(reducer);
}