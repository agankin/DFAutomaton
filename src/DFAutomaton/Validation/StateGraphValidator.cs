using Optional;

namespace DFAutomaton;

/// <summary>
/// Automaton states graph validator.
/// </summary>
internal static class StateGraphValidator<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// Validates states graph.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="start">States graph start.</param>
    /// <returns>Returns states graph start or validation error.</returns>
    public static Option<IState<TTransition, TState>, StateError> ValidateAnyReachAccepted(IState<TTransition, TState> start)
    {
        var canReachAcceptedStates = new HashSet<IState<TTransition, TState>>();
        var someCannotReachAccepted = StateVisitor<TTransition, TState>.Visit(start, StopWhenCannotReachAccepted(canReachAcceptedStates));
        
        if (someCannotReachAccepted)
            return Option.None<IState<TTransition, TState>, StateError>(StateError.NoAccepted);

        return start.Some<IState<TTransition, TState>, StateError>();
    }

    private static Func<IState<TTransition, TState>, VisitResult> StopWhenCannotReachAccepted(ISet<IState<TTransition, TState>> canReachAcceptedStates)
    {
        return state => CanReachAccepted(state, canReachAcceptedStates) ? VisitResult.Continue : VisitResult.Stop;
    }

    private static bool CanReachAccepted(IState<TTransition, TState> state, ISet<IState<TTransition, TState>> canReachAcceptedStates)
    {
        if (canReachAcceptedStates.Contains(state))
            return true;

        var visitedStates = new HashSet<IState<TTransition, TState>>();
        var canReachAccepted = StateVisitor<TTransition, TState>.Visit(state, StopWhenAccepted(visitedStates));

        if (canReachAccepted)
            Copy(visitedStates, canReachAcceptedStates);

        return canReachAccepted;
    }

    private static Func<IState<TTransition, TState>, VisitResult> StopWhenAccepted(ISet<IState<TTransition, TState>> visitedStates)
    {
        return state =>
        {
            visitedStates.Add(state);
            return state.Type == StateType.Accepted ? VisitResult.Stop : VisitResult.Continue;
        };
    }

    private static void Copy<TItem>(IEnumerable<TItem> source, ISet<TItem> destination)
    {
        foreach (var item in source)
            destination.Add(item);
    }
}