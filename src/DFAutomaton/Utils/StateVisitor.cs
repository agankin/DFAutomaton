using Optional;
using Optional.Unsafe;

namespace DFAutomaton;

/// <summary>
/// Utility to traverse through a states graph.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
internal static class StateVisitor<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// Traverses through state graph.
    /// </summary>
    /// <param name="start">State graph start state.</param>
    /// <param name="visit">Visit function.</param>
    /// <returns>True if was stopped by visit function, otherwise false.</returns>
    public static bool Visit(IState<TTransition, TState> start, Func<IState<TTransition, TState>, VisitResult> visit)
    {
        var visitedStates = new HashSet<IState<TTransition, TState>>();
        var statesToVisit = new Stack<IState<TTransition, TState>>();

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

    private static IEnumerable<IState<TTransition, TState>> GetNextStates(IState<TTransition, TState> state)
    {
        return state.Transitions
            .Select(transition => state[transition].ValueOrFailure().State)
            .Where(state => state.HasValue)
            .Select(state => state.ValueOrFailure());
    }
}

/// <summary>
/// Visit function result.
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