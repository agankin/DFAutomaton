namespace DFAutomaton;

/// <summary>
/// A builder for creating a fixed fallback transition.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="FromState">A state the transition originates from.</param>
/// <param name="Reducer">A delegate reducing state values.</param>
/// <remarks>
/// Fallback transition is a per-state single transition that is invoked for all unknown transition values.
/// </remarks>
public record FallbackFixedTransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState,
    Reduce<TTransition, TState> Reducer
) where TTransition : notnull
{
    /// <summary>
    /// Adds a fallback transition and completes the build.
    /// </summary>
    /// <returns>A new instance of <see cref="State{TTransition, TState}"/>.</returns>
    public State<TTransition, TState> ToNew()
    {
        var toState = FromState.OwningGraph.CreateState();
        FromState.AddFallbackTransition(toState.Id, Reducer);

        return toState;
    }

    /// <summary>
    /// Adds a fallback transition to an existing state and completes the build.
    /// </summary>
    /// <param name="toState">An existing state.</param>
    /// <returns>The instance of <see cref="State{TTransition, TState}"/>.</returns>
    public State<TTransition, TState> To(State<TTransition, TState> toState)
    {
        FromState.AddFallbackTransition(toState.Id, Reducer);
        return toState;
    }

    /// <summary>
    /// Adds a fallback transition to the same state it originates from and completes the build.
    /// </summary>
    /// <returns>The instance of <see cref="State{TTransition, TState}"/>.</returns>
    public State<TTransition, TState> ToSelf()
    {
        var toState = FromState;
        FromState.AddFallbackTransition(toState.Id, Reducer);

        return toState;
    }

    /// <summary>
    /// Adds a fallback transition to the accepted state and completes the build.
    /// </summary>
    /// <returns>The instance of <see cref="AcceptedState{TTransition, TState}"/>.</returns>
    public AcceptedState<TTransition, TState> ToAccepted()
    {
        var toStateId = StateId.AcceptedStateId;
        FromState.AddFallbackTransition(toStateId, Reducer);

        return new AcceptedState<TTransition, TState>(toStateId, FromState.OwningGraph);
    }
}