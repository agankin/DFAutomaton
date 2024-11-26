namespace DFAutomaton;

/// <summary>
/// A builder for creating a fallback transition.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="FromState">A state the transition originates from.</param>
/// <remarks>
/// Fallback transition is a per-state single transition that is invoked for all unknown transition values.
/// </remarks>
public record FallbackTransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState
) where TTransition : notnull
{
    /// <summary>
    /// Creates a fixed fallback transition builder.
    /// </summary>
    /// <param name="reducer">A state reducer.</param>
    /// <returns>A new instance of <see cref="FallbackFixedTransitionBuilder{TTransition, TState}"/>.</returns>
    public FallbackFixedTransitionBuilder<TTransition, TState> WithReducingBy(Reduce<TTransition, TState> reducer) =>
        new(FromState, reducer);

    /// <summary>
    /// Creates a dynamic fallback transition builder.
    /// </summary>
    /// <returns>A new instance of <see cref="FallbackDynamicTransitionBuilder{TTransition, TState}"/>.</returns>
    public FallbackDynamicTransitionBuilder<TTransition, TState> Dynamicly() => new(FromState);
}