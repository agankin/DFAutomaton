namespace DFAutomaton;

internal record TransitionPredicate<TTransition, TState>(
    string? Name,
    Predicate<TTransition> Predicate,
    TransitionEntry<TTransition, TState> Transition
)
where TTransition : notnull;