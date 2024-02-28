namespace DFAutomaton;

/// <summary>
/// A builder for creating new dynamic transitions.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="FromState">A state the new transition is originating from.</param>
/// <param name="Transition">A transition value.</param>
public record DynamicTransitionConfiguration<TTransition, TState>(
    State<TTransition, TState> FromState,
    TTransition Transition
) where TTransition : notnull
{
    /// <summary>
    /// Creates a new dynamic transition and completes build.
    /// </summary>
    /// <param name="reducer">An automaton transition reducer.</param>
    public void WithReducing(Reduce<TTransition, TState> reducer) => FromState.AddDynamicTransition(Transition, reducer);
}