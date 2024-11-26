using PureMonads;

namespace DFAutomaton;

/// <summary>
/// A builder for declaring new transitions.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="FromState">A state the transition originates from.</param>
/// <param name="ByValueOrPredicate">
/// Contains a transition value or predicate the transition will be selected by.
/// </param>
public record TransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState,
    Either<TTransition, CanTransit<TTransition>> ByValueOrPredicate
) where TTransition : notnull
{
    /// <summary>
    /// Creates a fixed transition builder.
    /// </summary>
    /// <param name="toValue">A new state value after reduce.</param>
    /// <returns>A new instance of <see cref="FixedTransitionBuilder{TTransition, TState}"/>.</returns>
    public FixedTransitionBuilder<TTransition, TState> WithReducingTo(TState toValue)
    {
        Reduce<TTransition, TState> reducer = (_, _) => toValue;
        return new(FromState, ByValueOrPredicate, reducer);
    }

    /// <summary>
    /// Creates a fixed transition builder.
    /// </summary>
    /// <param name="reducer">A delegate reducing state values.</param>
    /// <returns>A new instance of <see cref="FixedTransitionBuilder{TTransition, TState}"/>.</returns>
    public FixedTransitionBuilder<TTransition, TState> WithReducingBy(Reduce<TTransition, TState> reducer)
    {
        return new(FromState, ByValueOrPredicate, reducer);
    }

    /// <summary>
    /// Creates a dynamic transition builder.
    /// </summary>
    /// <returns>A new instance of <see cref="DynamicTransitionBuilder{TTransition, TState}"/>.</returns>
    public DynamicTransitionBuilder<TTransition, TState> Dynamicly() => new(FromState, ByValueOrPredicate);
}