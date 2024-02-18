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
    public FixedTransitionConfiguration<TTransition, TState> WithReducing(TState toValue)
    {
        var reduce = ReducerFactory.Create<TTransition, TState>(toValue);
        return new(FromState, Transition, reduce);
    }

    /// <summary>
    /// Creates a new fixed transition builder.
    /// </summary>
    /// <param name="reduceValue">A state value reducer.</param>
    /// <returns>The created fixed transition builder.</returns>
    public FixedTransitionConfiguration<TTransition, TState> WithReducing(ReduceValue<TTransition, TState> reduceValue)
    {
        var reduce = ReducerFactory.Create(reduceValue);
        return new(FromState, Transition, reduce);
    }

    /// <summary>
    /// Creates a new fixed transition builder.
    /// </summary>
    /// <param name="reduceTransition">A state value reducer.</param>
    /// <returns>The created fixed transition builder.</returns>
    public FixedTransitionConfiguration<TTransition, TState> WithReducing(ReduceTransition<TTransition, TState> reduceTransition)
    {
        var reduce = ReducerFactory.Create(reduceTransition);
        return new(FromState, Transition, reduce);
    }

    /// <summary>
    /// Creates a new dynamic transition builder.
    /// </summary>
    /// <returns>The created dynamic transition builder.</returns>
    public DynamicTransitionConfiguration<TTransition, TState> Dynamicly() => new(FromState, Transition);
}