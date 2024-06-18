namespace DFAutomaton;

/// <summary>
/// Contains a transition value or predicate and a transition to a next state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="TransitionValueOrPredicate">A transition value or predicate.</param>
/// <param name="Transition">A transition to a next state.</param>
public record StateTransition<TTransition, TState>(
    Either<TTransition, Predicate<TTransition>> TransitionValueOrPredicate,
    Transition<TTransition, TState> Transition
)
where TTransition : notnull;