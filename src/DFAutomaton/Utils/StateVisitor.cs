using Optional;

namespace DFAutomaton;

internal static class StateVisitor<TTransition, TState> where TTransition : notnull
{
    public static Option<TResult> VisitTillResult<TResult>(
        IState<TTransition, TState> start,
        Func<IState<TTransition, TState>, Option<TResult>> visit)
    {
        var visitedStates = new HashSet<IState<TTransition, TState>>();
        return VisitTillResult(start, visit, visitedStates);
    }

    private static Option<TResult> VisitTillResult<TResult>(
        IState<TTransition, TState> state,
        Func<IState<TTransition, TState>, Option<TResult>> visit,
        ISet<IState<TTransition, TState>> visitedStates)
    {
        if (visitedStates.Contains(state))
            return Option.None<TResult>();

        visitedStates.Add(state);

        return visit(state).Else(() => VisitNextStatesTillResult(state, visit, visitedStates));
    }

    private static Option<TResult> VisitNextStatesTillResult<TResult>(
        IState<TTransition, TState> state,
        Func<IState<TTransition, TState>, Option<TResult>> visit,
        ISet<IState<TTransition, TState>> visitedStates)
    {
        return state.Transitions.Select(transition => state[transition])
            .Aggregate(
                Option.None<TResult>(),
                (result, moveOption) => ResultOrVisitNext(result, moveOption, visit, visitedStates));
    }

    private static Option<TResult> ResultOrVisitNext<TResult>(
        Option<TResult> result,
        Option<IState<TTransition, TState>.Move> moveOption,
        Func<IState<TTransition, TState>, Option<TResult>> visit,
        ISet<IState<TTransition, TState>> visitedStates)
    {
        return result.Else(
            moveOption.FlatMap(
                move => VisitTillResult(move.NextState, visit, visitedStates)));
    }
}