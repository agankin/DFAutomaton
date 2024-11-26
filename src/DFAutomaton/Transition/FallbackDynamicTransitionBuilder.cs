using PureMonads;

namespace DFAutomaton;

/// <summary>
/// A builder for creating a dynamic fallback transition.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="FromState">A state the transition originates from.</param>
/// <remarks>
/// Fallback transition is a per-state signle transition that is invoked for all unknown transition values.
/// </remarks>
public record FallbackDynamicTransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState
) where TTransition : notnull
{
    /// <summary>
    /// Adds a new dynamic fallback transition and completes the build.
    /// </summary>
    /// <param name="reducer">A transition reducer.</param>
    public void WithReducingBy(Reduce<TTransition, TState> reducer) =>
        FromState.AddFallbackTransition(Option.None<StateId>(), reducer);
}