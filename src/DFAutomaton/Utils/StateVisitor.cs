using Optional;

namespace DFAutomaton;

/// <summary>
/// Utility to traverse through state graphs.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
internal static class StateVisitor<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// Traverses through state graph until result found.
    /// </summary>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <param name="start">State graph start state.</param>
    /// <param name="visit">Function returning Some result or None.</param>
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
                (result, transitionOption) => ResultOrVisitNext(result, transitionOption, visit, visitedStates));
    }

    private static Option<TResult> ResultOrVisitNext<TResult>(
        Option<TResult> result,
        Option<IState<TTransition, TState>.Transition> transitionOption,
        Func<IState<TTransition, TState>, Option<TResult>> visit,
        ISet<IState<TTransition, TState>> visitedStates)
    {
        return result.Else(
            transitionOption.FlatMap(
                transition => transition.State.FlatMap(
                    state => VisitTillResult(state, visit, visitedStates))));
    }
}