using PureMonads;

namespace DFAutomaton;

/// <summary>
/// A builder for creating a fixed transition.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="FromState">A state the transition originates from.</param>
/// <param name="ByValueOrPredicate">
/// Contains a transition value or predicate the transition will be selected by.
/// </param>
/// <param name="Reducer">A delegate reducing state values.</param>
public record FixedTransitionBuilder<TTransition, TState>(
    State<TTransition, TState> FromState,
    Either<TTransition, InputPredicate<TTransition>> ByValueOrPredicate,
    Reduce<TTransition, TState> Reducer
)
where TTransition : notnull
{
    /// <summary>
    /// Adds a fixed transition and completes the state transition build.
    /// </summary>
    /// <returns>A new instance of <see cref="State{TTransition, TState}"/>.</returns>
    public State<TTransition, TState> GoToNewState()
    {
        var toState = FromState.OwningGraph.CreateState();
        return FromState.AddFixedTransition(toState.Id, ByValueOrPredicate, Reducer);
    }

    /// <summary>
    /// Adds a fixed transition and completes the state transition build.
    /// </summary>
    /// <param name="toState">An existing state.</param>
    /// <returns>The instance of <see cref="State{TTransition, TState}"/>.</returns>
    public State<TTransition, TState> GoTo(State<TTransition, TState> toState) =>
        FromState.AddFixedTransition(toState.Id, ByValueOrPredicate, Reducer);

    /// <summary>
    /// Adds a fixed transition and completes the state transition build.
    /// </summary>
    /// <returns>The instance of <see cref="State{TTransition, TState}"/>.</returns>
    public State<TTransition, TState> GoToSelf()
    {
        var toState = FromState;
        return FromState.AddFixedTransition(toState.Id, ByValueOrPredicate, Reducer);
    }

    /// <summary>
    /// Adds a fixed transition and completes the state transition build.
    /// </summary>
    /// <returns>The instance of <see cref="AcceptedState{TTransition, TState}"/>.</returns>
    public AcceptedState<TTransition, TState> GoToAccepted()
    {
        var toStateId = StateId.AcceptedStateId;
        var acceptedState = FromState.AddFixedTransition(toStateId, ByValueOrPredicate, Reducer);

        return new AcceptedState<TTransition, TState>(acceptedState.Id, acceptedState.OwningGraph);
    }
}