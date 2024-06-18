using PureMonads;

namespace DFAutomaton;

internal delegate VisitResult Visit<TTransition, TState>(State<TTransition, TState> state)
    where TTransition : notnull;

internal static class StateVisitor<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// Traverses a state graph calling a visitor function against each node.
    /// </summary>
    /// <param name="start">The start state of a state graph.</param>
    /// <param name="visit">A visitor function.</param>
    /// <returns>
    /// Boolean value. True value denotes the visitor function returned VisitResult.Stop at a node stopping traversal process. Otherwise returns false.
    /// </returns>
    public static bool Visit(State<TTransition, TState> start, Visit<TTransition, TState> visit)
    {
        var visitedStates = new HashSet<State<TTransition, TState>>();
        var statesToVisit = new Stack<State<TTransition, TState>>();

        statesToVisit.Push(start);

        while (statesToVisit.TryPop(out var currentState))
        {
            var result = visit(currentState);
            if (result == VisitResult.Stop)
                return true;

            visitedStates.Add(currentState);

            var nextStates = GetNextStates(currentState).Where(state => !visitedStates.Contains(state));
            foreach (var nextState in nextStates)
                statesToVisit.Push(nextState);
        }

        return false;
    }

    private static IEnumerable<State<TTransition, TState>> GetNextStates(State<TTransition, TState> state)
    {
        return state.Transitions
            .Select(stateTransition => stateTransition.Transition.ToState)
            .Where(state => state.HasValue)
            .Select(state => state.ValueOrFailure());
    }
}

/// <summary>
/// Contains possible results of visitor functions calls.
/// </summary>
internal enum VisitResult
{
    /// <summary>
    /// Continue visiting.
    /// </summary>
    Continue = 1,

    /// <summary>
    /// Stop visiting.
    /// </summary>
    Stop
}