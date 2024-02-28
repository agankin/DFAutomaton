namespace DFAutomaton;

/// <summary>
/// A builder for creating new fixed transitions.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="FromState">A state the new transition is originating from.</param>
/// <param name="Transition">A transition value.</param>
/// <param name="Reducer">An automaton transition reducer.</param>
public record FixedTransitionConfiguration<TTransition, TState>(
    State<TTransition, TState> FromState,
    TTransition Transition,
    Reduce<TTransition, TState> Reducer
)
where TTransition : notnull
{
    /// <summary>
    /// Creates a new transition to a new state and completes build with returning the created new state.
    /// </summary>
    /// <returns>The created new state.</returns>
    public State<TTransition, TState> ToNew() =>
        FromState.AddFixedTransitionToNewState(Transition, Reducer);

    /// <summary>
    /// Creates a transition to the provided existing state and completes build.
    /// </summary>
    /// <param name="toState">An existing state.</param>
    /// <returns>The provided existing state.</returns>
    public State<TTransition, TState> To(State<TTransition, TState> toState) =>
        FromState.AddFixedTransition(toState.Id, Transition, Reducer);

    /// <summary>
    /// Creates a transition to the same state it originating from and completes build.
    /// </summary>
    /// <returns>The same state the new transition is originating from.</returns>
    public State<TTransition, TState> ToSelf()
    {
        var toState = FromState;
        return FromState.AddFixedTransition(toState.Id, Transition, Reducer);
    }

    /// <summary>
    /// Creates a transition to the accepted state and completes build.
    /// </summary>
    public void ToAccepted()
    {
        var toStateId = StateId.AcceptedStateId;
        FromState.AddFixedTransition(toStateId, Transition, Reducer);
    }
}