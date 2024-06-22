namespace DFAutomaton;

/// <summary>
/// A builder for declaring new transitions.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="FromState">A state the new transition is starting from.</param>
/// <param name="ByValueOrPredicate">Contains a value or a predicate the transition is performed by.</param>
public record TransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState,
    Either<TTransition, CanTransit<TTransition>> ByValueOrPredicate
) where TTransition : notnull
{
    /// <summary>
    /// Creates a new fixed transition builder.
    /// </summary>
    /// <param name="toValue">A new state value after the transition.</param>
    /// <returns>The created fixed transition builder.</returns>
    public FixedTransitionBuilder<TTransition, TState> WithReducingTo(TState toValue)
    {
        Reduce<TTransition, TState> reducer = (_, _) => toValue;
        return new(FromState, ByValueOrPredicate, reducer);
    }

    /// <summary>
    /// Creates a new fixed transition builder.
    /// </summary>
    /// <param name="reducer">A reducer.</param>
    /// <returns>The created fixed transition builder.</returns>
    public FixedTransitionBuilder<TTransition, TState> WithReducingBy(Reduce<TTransition, TState> reducer)
    {
        return new(FromState, ByValueOrPredicate, reducer);
    }

    /// <summary>
    /// Creates a new dynamic transition builder.
    /// </summary>
    /// <returns>The created dynamic transition builder.</returns>
    public DynamicTransitionBuilder<TTransition, TState> Dynamicly() => new(FromState, ByValueOrPredicate);
}