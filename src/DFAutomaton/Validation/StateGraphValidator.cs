using PureMonads;

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
    /// <returns>Some validation error or None.</returns>
    public static Option<ValidationError> ValidateHasAccepted(StateGraph<TTransition, TState> stateGraph)
    {
        var startState = stateGraph.StartState;
        var acceptedReached = StateVisitor<TTransition, TState>.Visit(startState, StopWhenReachedAccepted);
        
        if (!acceptedReached)
            return ValidationError.NoAccepted;

        return Option.None<ValidationError>();
    }

    /// <summary>
    /// Validates that any state can reach the accepted state (checks only states with fixed transitions).
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="start">The start state of a state graph.</param>
    /// <returns>Some validation error or None.</returns>
    public static Option<ValidationError> ValidateAnyReachAccepted(StateGraph<TTransition, TState> stateGraph)
    {
        var canReachAcceptedStates = new HashSet<State<TTransition, TState>>();
        var startState = stateGraph.StartState;
        var someCannotReachAccepted = StateVisitor<TTransition, TState>.Visit(startState, StopWhenCannotReachAccepted(canReachAcceptedStates));
        
        if (someCannotReachAccepted)
            return ValidationError.AcceptedIsUnreachable;

        return Option.None<ValidationError>();
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

        Visit<TTransition, TState> StopWhenCanReachAccepted_() => StopWhenCanReachAccepted(canReachAcceptedStates);
        var canReachAccepted = StateVisitor<TTransition, TState>.Visit(state, StopWhenCanReachAccepted_());
        if (canReachAccepted)
            canReachAcceptedStates.Add(state);

        return canReachAccepted;
    }

    private static Visit<TTransition, TState> StopWhenCanReachAccepted(ISet<State<TTransition, TState>> canReachAcceptedStates)
    {
        return state =>
        {
            return canReachAcceptedStates.Contains(state) || state.Type == StateType.Accepted
                ? VisitResult.Stop
                : VisitResult.Continue;
        };
    }
}