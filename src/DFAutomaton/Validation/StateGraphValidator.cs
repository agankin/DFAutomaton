namespace DFAutomaton;

/// <summary>
/// Contains methods for state graph validation.
/// </summary>
internal static class StateGraphValidator<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// Validates the provided state graph contains the accepted state.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="start">The start state of a state graph.</param>
    /// <returns>The result of the validation.</returns>
    public static ValidationResult<TTransition, TState> ValidateHasAccepted(State<TTransition, TState> start)
    {
        var acceptedReached = StateVisitor<TTransition, TState>.Visit(start, StopWhenReachedAccepted);
        
        if (!acceptedReached)
            return ValidationError.NoAccepted;

        return start;
    }

    /// <summary>
    /// Validates that any state can reach the accepted state (checks only states with fixed transitions).
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="start">The start state of a state graph.</param>
    /// <returns>The result of the validation.</returns>
    public static ValidationResult<TTransition, TState> ValidateAnyReachAccepted(State<TTransition, TState> start)
    {
        var canReachAcceptedStates = new HashSet<State<TTransition, TState>>();
        var someCannotReachAccepted = StateVisitor<TTransition, TState>.Visit(start, StopWhenCannotReachAccepted(canReachAcceptedStates));
        
        if (someCannotReachAccepted)
            return ValidationError.AcceptedIsUnreachable;

        return start;
    }

    private static VisitResult StopWhenReachedAccepted(State<TTransition, TState> state) =>
        state.Type == StateType.Accepted ? VisitResult.Stop : VisitResult.Continue;

    private static Visit<TTransition, TState> StopWhenCannotReachAccepted(ISet<State<TTransition, TState>> canReachAcceptedStates)
    {
        return state => CanReachAccepted(state, canReachAcceptedStates) ? VisitResult.Continue : VisitResult.Stop;
    }

    private static bool CanReachAccepted(State<TTransition, TState> state, ISet<State<TTransition, TState>> canReachAcceptedStates)
    {
        if (canReachAcceptedStates.Contains(state))
            return true;

        var visitedStates = new HashSet<State<TTransition, TState>>();
        var canReachAccepted = StateVisitor<TTransition, TState>.Visit(state, StopWhenAccepted(visitedStates));

        if (canReachAccepted)
            Copy(visitedStates, canReachAcceptedStates);

        return canReachAccepted;
    }

    private static Visit<TTransition, TState> StopWhenAccepted(ISet<State<TTransition, TState>> visitedStates)
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