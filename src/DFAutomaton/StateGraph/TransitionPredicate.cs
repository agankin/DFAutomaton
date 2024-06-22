namespace DFAutomaton;

internal record TransitionPredicate<TTransition, TState>(
    string? Name,
    Predicate<TTransition> Predicate,
    TransitionEntry<TTransition, TState> Transition
)
where TTransition : notnull;

internal static class TransitionPredicateSearchingExtensions
{
    public static bool TryFindTransition<TTransition, TState>(
        this IDictionary<StateId, List<TransitionPredicate<TTransition, TState>>> transitionPredicatesByStateId,
        StateId fromStateId,
        TTransition transition,
        out TransitionEntry<TTransition, TState> transitionEntry)
        where TTransition : notnull
    {
        transitionEntry = default;

        if (!transitionPredicatesByStateId.TryGetValue(fromStateId, out var predicates))
            return false;

        return predicates.TryFindTransition(transition, out transitionEntry);
    }

    public static bool TryFindTransition<TTransition, TState>(
        this ILookup<StateId, TransitionPredicate<TTransition, TState>> transitionPredicatesByStateId,
        StateId fromStateId,
        TTransition transition,
        out TransitionEntry<TTransition, TState> transitionEntry)
        where TTransition : notnull
    {
        var predicates = transitionPredicatesByStateId[fromStateId];

        return predicates.TryFindTransition(transition, out transitionEntry);
    }

    private static bool TryFindTransition<TTransition, TState>(
        this IEnumerable<TransitionPredicate<TTransition, TState>> predicates,
        TTransition transition,
        out TransitionEntry<TTransition, TState> transitionEntry)
        where TTransition : notnull
    {
        foreach (var entry in predicates)
        {
            if (entry.Predicate(transition))
            {
                transitionEntry = entry.Transition;
                return true;
            }
        }

        transitionEntry = default;
        return false;
    }
}