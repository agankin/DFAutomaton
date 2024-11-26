using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Contains a transition value or predicate a transition will be selected by
/// and information about transition to a next state.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="ByValueOrPredicate">
/// Contains a transition value or a predicate the transition will be selected by.
/// /// </param>
/// <param name="Transition">Information about transition to a next state.</param>
public record StateTransition<TTransition, TState>(
    Either<TTransition, CanTransit<TTransition>> ByValueOrPredicate,
    Transition<TTransition, TState> Transition
)
where TTransition : notnull;