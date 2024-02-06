using Optional;
using Optional.Unsafe;

namespace DFAutomaton.Utils;

/// <summary>
/// The formatter for obtaining text representations of state graphs.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public static class StatesGraphFormatter<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// Returns a text representation of the provided state graph.
    /// </summary>
    /// <param name="start">The start state of a state graph.</param>
    /// <returns>Text representation of the provided state graph.</returns>
    public static string Format(IState<TTransition, TState> start)
    {
        var formattedStates = new HashSet<IState<TTransition, TState>>();
        var lines = GetStateLines(start, formattedStates);

        return string.Join(Environment.NewLine, lines);
    }

    private static IEnumerable<string> GetStateLines(
        IState<TTransition, TState> state,
        ISet<IState<TTransition, TState>> formattedStates)
    {
        if (formattedStates.Contains(state))
            yield break;
        formattedStates.Add(state);

        yield return FormatState(state);
        
        var formattedTransitions = state.Transitions.Select(FormatTransition(state));
        foreach (var formattedTransition in formattedTransitions)
            yield return formattedTransition;

        yield return string.Empty;

        var nextStates = state.Transitions.Select(transition => state[transition].ValueOrFailure().State)
            .Where(nextState => nextState.HasValue)
            .Select(nextStateOption => nextStateOption.ValueOrFailure());
        var nextStatesLines = nextStates.SelectMany(nextState => GetStateLines(nextState, formattedStates));
        foreach (var nextStateLine in nextStatesLines)
            yield return nextStateLine;
    }

    private static Func<TTransition, string> FormatTransition(IState<TTransition, TState> fromState)
    {
        return transition =>
        {
            var (_, toState, _) = fromState[transition].ValueOrFailure();

            return FormatTransition(transition, toState);
        };
    }

    private static string FormatState(IState<TTransition, TState> state) => state.Format();

    private static string FormatTransition(TTransition transition, Option<IState<TTransition, TState>> toStateOption)
    {
        var toStateFormatted = toStateOption.Map(toState => toState.Id.ToString()).ValueOr("DYNAMIC GOTO");
        return $"    {transition} -> State {toStateFormatted}";
    }
}