namespace DFAutomaton;

/// <summary>
/// A builder for declaring new transitions.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="FromState">A state the new transition is originating from.</param>
/// <param name="Transition">A transition value.</param>
public record TransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState,
    TTransition Transition
) where TTransition : notnull
{
    /// <summary>
    /// Creates a new fixed transition builder.
    /// </summary>
    /// <param name="toValue">A state value after the transition performed.</param>
    /// <returns>The created fixed transition builder.</returns>
    public FixedTransitionConfiguration<TTransition, TState> WithReducingTo(TState toValue)
    {
        Reduce<TTransition, TState> reduce = (_, _) => toValue;
        return new(FromState, Transition, reduce);
    }

    /// <summary>
    /// Creates a new fixed transition builder.
    /// </summary>
    /// <param name="reduceValue">A reducer.</param>
    /// <returns>The created fixed transition builder.</returns>
    public FixedTransitionConfiguration<TTransition, TState> WithReducingBy(Reduce<TTransition, TState> reduce)
    {
        return new(FromState, Transition, reduce);
    }

    /// <summary>
    /// Creates a new dynamic transition builder.
    /// </summary>
    /// <returns>The created dynamic transition builder.</returns>
    public DynamicTransitionConfiguration<TTransition, TState> Dynamicly() => new(FromState, Transition);
}