using Optional;

namespace DFAutomaton;

internal static class AutomatonStateGraphValidator
{
    public static Option<IState<TTransition, TState>, StateError> ValidateAnyReachAccepted<TTransition, TState>(this IState<TTransition, TState> start)
        where TTransition : notnull
    {
        var statesReachingAccepted = new HashSet<IState<TTransition, TState>>();

        var hasAccepted = StateVisitor<TTransition, TState>
            .VisitTillResult(
                start,
                state => state.Type == StateType.Accepted ? true.Some() : Option.None<bool>())
            .ValueOr(false);

        if (!hasAccepted)
            return Option.None<IState<TTransition, TState>, StateError>(StateError.NoAccepted);

        var errorOption = StateVisitor<TTransition, TState>.VisitTillResult(
            start,
            state => CanReachAccepted(state, statesReachingAccepted)
                ? Option.None<StateError>()
                : StateError.AcceptedIsUnreachable.Some());

        return errorOption
            .Map(Option.None<IState<TTransition, TState>, StateError>)
            .ValueOr(start.Some<IState<TTransition, TState>, StateError>);
    }

    private static bool CanReachAccepted<TTransition, TState>(
        IState<TTransition, TState> state,
        ISet<IState<TTransition, TState>> statesReachingAccepted)
        where TTransition : notnull
    {
        var visitedStates = new HashSet<IState<TTransition, TState>>();

        return CanReachAccepted(state, visitedStates, statesReachingAccepted);
    }

    private static bool CanReachAccepted<TTransition, TState>(
        IState<TTransition, TState> state,
        ISet<IState<TTransition, TState>> visitedStates,
        ISet<IState<TTransition, TState>> statesReachingAccepted)
        where TTransition : notnull
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
                .Map(move => CanReachAccepted(move.NextState, visitedStates, statesReachingAccepted))
                .ValueOr(false));

        if (canReachAccepted)
            statesReachingAccepted.Add(state);

        return canReachAccepted;
    }
}