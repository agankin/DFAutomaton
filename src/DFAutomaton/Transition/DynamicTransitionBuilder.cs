namespace DFAutomaton;

/// <summary>
/// A builder for creating new dynamic transitions.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="FromState">A state the new transition is starting from.</param>
/// <param name="ByValueOrPredicate">Contains a value or a predicate the transition is performed by.</param>
public record DynamicTransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState,
    Either<TTransition, CanTransit<TTransition>> ByValueOrPredicate
) where TTransition : notnull
{
    /// <summary>
    /// Adds a new dynamic transition and completes the build.
    /// </summary>
    /// <param name="reducer">An automaton transition reducer.</param>
    public void WithReducing(Reduce<TTransition, TState> reducer) => FromState.AddDynamicTransition(ByValueOrPredicate, reducer);
}