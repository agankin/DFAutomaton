namespace DFAutomaton;

/// <summary>
/// A builder for creating new fixed transitions.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="FromState">A state the new transition is starting from.</param>
/// <param name="ByValueOrPredicate">Contains a value or a predicate the transition is performed by.</param>
/// <param name="Reducer">An automaton transition reducer.</param>
public record FixedTransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState,
    Either<TTransition, CanTransit<TTransition>> ByValueOrPredicate,
    Reduce<TTransition, TState> Reducer
)
where TTransition : notnull
{
    /// <summary>
    /// Adds a new transition to a new state and completes the build with returning the created new state.
    /// </summary>
    /// <returns>The created new state.</returns>
    public State<TTransition, TState> ToNew()
    {
        var toState = FromState.OwningGraph.CreateState();
        return FromState.AddFixedTransition(toState.Id, ByValueOrPredicate, Reducer);
    }

    /// <summary>
    /// Adds a transition to the provided existing state and completes the build.
    /// </summary>
    /// <param name="toState">An existing state.</param>
    /// <returns>The provided existing state.</returns>
    public State<TTransition, TState> To(State<TTransition, TState> toState) =>
        FromState.AddFixedTransition(toState.Id, ByValueOrPredicate, Reducer);

    /// <summary>
    /// Adds a transition to the same state it starting from and completes the build.
    /// </summary>
    /// <returns>The same state the new transition is starting from.</returns>
    public State<TTransition, TState> ToSelf()
    {
        var toState = FromState;
        return FromState.AddFixedTransition(toState.Id, ByValueOrPredicate, Reducer);
    }

    /// <summary>
    /// Adds a new transition to the accepted state and completes the build.
    /// </summary>
    public AcceptedState<TTransition, TState> ToAccepted()
    {
        var toStateId = StateId.AcceptedStateId;
        var acceptedState = FromState.AddFixedTransition(toStateId, ByValueOrPredicate, Reducer);

        return new AcceptedState<TTransition, TState>(acceptedState.Id, acceptedState.OwningGraph);
    }
}