using Optional;

namespace DFAutomaton;

internal static class AutomatonStateGraphValidator<TTransition, TState> where TTransition : notnull
{
    public static Option<IState<TTransition, TState>, AutomatonGraphError> ValidateAnyReachAccepted(IState<TTransition, TState> startState)
    {
        var statesReachingAccepted = new HashSet<IState<TTransition, TState>>();

        var hasAccepted = StateGraphVisitor<TTransition, TState>
            .VisitTillResult(
                startState,
                state => state.Type == StateType.Accepted
                    ? true.Some()
                    : Option.None<bool>())
            .ValueOr(false);

        if (!hasAccepted)
            return Option.None<IState<TTransition, TState>, AutomatonGraphError>(AutomatonGraphError.NoAccepted);

        var errorOption = StateGraphVisitor<TTransition, TState>.VisitTillResult(
            startState,
            state => CanReachAccepted(state, statesReachingAccepted)
                ? Option.None<AutomatonGraphError>()
                : AutomatonGraphError.AcceptedIsUnreachable.Some());

        return errorOption
            .Map(Option.None<IState<TTransition, TState>, AutomatonGraphError>)
            .ValueOr(startState.Some<IState<TTransition, TState>, AutomatonGraphError>);
    }

    private static bool CanReachAccepted(
        IState<TTransition, TState> state,
        ISet<IState<TTransition, TState>> statesReachingAccepted)
    {
        var visitedStates = new HashSet<IState<TTransition, TState>>();

        return CanReachAccepted(state, visitedStates, statesReachingAccepted);
    }

    private static bool CanReachAccepted(
        IState<TTransition, TState> state,
        ISet<IState<TTransition, TState>> visitedStates,
        ISet<IState<TTransition, TState>> statesReachingAccepted)
    {
        if (state.Type == StateType.Accepted)
            return true;

        if (statesReachingAccepted.Contains(state))
            return true;

        if (visitedStates.Contains(state))
            return false;

        visitedStates.Add(state);

        var canReachAccepted = state.Transitions
            .Any(transition => state[transition]
                .Map(t => CanReachAccepted(t.NextState, visitedStates, statesReachingAccepted))
                .ValueOr(false));

        if (canReachAccepted)
            statesReachingAccepted.Add(state);

        return canReachAccepted;
    }
}