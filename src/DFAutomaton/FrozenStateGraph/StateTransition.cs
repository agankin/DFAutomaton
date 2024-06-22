namespace DFAutomaton;

/// <summary>
/// Contains a transition value or predicate and a transition to a next state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="ByValueOrPredicate">Contains a value or a predicate the transition is performed by.</param>
/// <param name="Transition">A transition to a next state.</param>
public record FrozenStateTransition<TTransition, TState>(
    Either<TTransition, CanTransit<TTransition>> ByValueOrPredicate,
    FrozenTransition<TTransition, TState> Transition
)
where TTransition : notnull;