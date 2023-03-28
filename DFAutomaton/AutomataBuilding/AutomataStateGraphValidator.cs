using Optional;

namespace DFAutomaton
{
    internal static class AutomataStateGraphValidator<TTransition, TState> where TTransition : notnull
    {
        public static Option<IState<TTransition, TState>, AutomataGraphError> ValidateAnyReachAccepted(IState<TTransition, TState> startState)
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
                return Option.None<IState<TTransition, TState>, AutomataGraphError>(AutomataGraphError.NoAccepted);

            var errorOption = StateGraphVisitor<TTransition, TState>.VisitTillResult(
                startState,
                state => CanReachAccepted(state, statesReachingAccepted)
                    ? Option.None<AutomataGraphError>()
                    : AutomataGraphError.AcceptedIsUnreachable.Some());

            return errorOption
                .Map(Option.None<IState<TTransition, TState>, AutomataGraphError>)
                .ValueOr(startState.Some<IState<TTransition, TState>, AutomataGraphError>);
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
}