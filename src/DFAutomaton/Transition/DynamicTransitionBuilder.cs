using PureMonads;

namespace DFAutomaton;

/// <summary>
/// A builder for creating a new dynamic transition.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="FromState">A state the transition originates from.</param>
/// <param name="ByValueOrPredicate">
/// Contains a transition value or predicate the transition will be selected by.
/// </param>
public record DynamicTransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState,
    Either<TTransition, CanTransit<TTransition>> ByValueOrPredicate
) where TTransition : notnull
{
    /// <summary>
    /// Adds a new dynamic transition and completes the build.
    /// </summary>
    /// <param name="reducer">A transition reducer.</param>
    public void WithReducingBy(Reduce<TTransition, TState> reducer) => FromState.AddDynamicTransition(ByValueOrPredicate, reducer);
}