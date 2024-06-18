namespace DFAutomaton;

/// <summary>
/// A builder for creating new dynamic transitions.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="FromState">A state the new transition is starting from.</param>
/// <param name="TransitionValueOrPredicate">A transition value or a transition predicate.</param>
public record DynamicTransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState,
    Either<TTransition, Predicate<TTransition>> TransitionValueOrPredicate
) where TTransition : notnull
{
    /// <summary>
    /// Adds a new dynamic transition and completes the build.
    /// </summary>
    /// <param name="reducer">An automaton transition reducer.</param>
    public void WithReducing(Reduce<TTransition, TState> reducer) => FromState.AddDynamicTransition(TransitionValueOrPredicate, reducer);
}