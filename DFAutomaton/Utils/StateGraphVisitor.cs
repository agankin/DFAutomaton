using Optional;

namespace DFAutomaton
{
    internal static class StateGraphVisitor<TTransition, TState> where TTransition : notnull
    {
        public static Option<TResult> VisitTillResult<TResult>(
            IState<TTransition, TState> startState,
            Func<IState<TTransition, TState>, Option<TResult>> visit)
        {
            var visitedStates = new HashSet<IState<TTransition, TState>>();
            return VisitTillResult(startState, visit, visitedStates);
        }

        private static Option<TResult> VisitTillResult<TResult>(
            IState<TTransition, TState> state,
            Func<IState<TTransition, TState>, Option<TResult>> visit,
            ISet<IState<TTransition, TState>> visitedStates)
        {
            if (visitedStates.Contains(state))
                return Option.None<TResult>();

            visitedStates.Add(state);

            return visit(state)
                .Else(() => VisitNextStatesTillResult(state, visit, visitedStates));
        }

        private static Option<TResult> VisitNextStatesTillResult<TResult>(
            IState<TTransition, TState> state,
            Func<IState<TTransition, TState>, Option<TResult>> visit,
            ISet<IState<TTransition, TState>> visitedStates)
        {
            return state.Transitions.Select(transition => state[transition])
                .Aggregate(
                    Option.None<TResult>(),
                    (result, transitionOption) => ResultOrVisitNext(result, transitionOption, visit, visitedStates));
        }

        private static Option<TResult> ResultOrVisitNext<TResult>(
            Option<TResult> result,
            Option<StateTransition<TTransition, TState>> transitionOption,
            Func<IState<TTransition, TState>, Option<TResult>> visit,
            ISet<IState<TTransition, TState>> visitedStates)
        {
            return result.Else(
                transitionOption.FlatMap(
                    transition => VisitTillResult(transition.NextState, visit, visitedStates)));
        }
    }
}