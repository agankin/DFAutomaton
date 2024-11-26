using PureMonads;

namespace DFAutomaton;

using static Option;

/// <summary>
/// Contains methods for state graph validation.
/// </summary>
internal static class StateGraphValidator<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// Validates a state graph has connections to the accepted state.
    /// </summary>
    /// <typeparam name="TTransition">The transition type.</typeparam>
    /// <typeparam name="TState">The state type.</typeparam>
    /// <param name="start">The start state of a state graph.</param>
    /// <returns><see cref="Option{ValidationError}"/> containing validation error or None.</returns>
    public static Option<ValidationError> ValidateHasAccepted(StateGraph<TTransition, TState> stateGraph)
    {
        var startState = stateGraph.StartState;
        var acceptedReached = StateVisitor<TTransition, TState>.Visit(startState, StopWhenReachedAccepted);
        
        if (!acceptedReached)
            return ValidationError.NoAccepted;

        return None<ValidationError>();
    }

    /// <summary>
    /// Validates that any state is connected to the accepted state (checks only states with fixed transitions).
    /// </summary>
    /// <typeparam name="TTransition">The transition type.</typeparam>
    /// <typeparam name="TState">The state type.</typeparam>
    /// <param name="start">The start state of a state graph.</param>
    /// <returns><see cref="Option{ValidationError}"/> containing validation error or None.</returns>
    public static Option<ValidationError> ValidateAnyReachAccepted(StateGraph<TTransition, TState> stateGraph)
    {
        var validStates = new HashSet<State<TTransition, TState>>();
        var startState = stateGraph.StartState;
        
        var someCannotReachAccepted = StateVisitor<TTransition, TState>.Visit(startState, StopWhenCannotReachAccepted(validStates));
        if (someCannotReachAccepted)
            return ValidationError.AcceptedIsUnreachable;

        return None<ValidationError>();
    }

    private static VisitResult StopWhenReachedAccepted(State<TTransition, TState> state) =>
        IsAccepted(state) || HasDynamicTransition(state) ? VisitResult.Stop : VisitResult.Continue;

    private static Visit<TTransition, TState> StopWhenCannotReachAccepted(ISet<State<TTransition, TState>> validStates)
    {
        return state => CanReachAccepted(state, validStates) ? VisitResult.Continue : VisitResult.Stop;
    }

    private static bool CanReachAccepted(State<TTransition, TState> state, ISet<State<TTransition, TState>> validStates)
    {
        if (validStates.Contains(state))
            return true;

        var reachesAccepted = StateVisitor<TTransition, TState>.Visit(state, StopWhenCanReachAccepted(validStates));
        if (reachesAccepted)
            validStates.Add(state);

        return reachesAccepted;
    }

    private static Visit<TTransition, TState> StopWhenCanReachAccepted(ISet<State<TTransition, TState>> validStates)
    {
        return state => validStates.Contains(state) || IsAccepted(state) || HasDynamicTransition(state)
            ? VisitResult.Stop
            : VisitResult.Continue;
    }

    private static bool IsAccepted(State<TTransition, TState> state) =>
        state.Type == StateType.Accepted;
    
    private static bool HasDynamicTransition(State<TTransition, TState> state) =>
        state.Transitions.Any(IsDynamicTransition);

    private static bool IsDynamicTransition(StateTransition<TTransition, TState> stateTransition) =>
        !stateTransition.Transition.ToState.HasValue;
}