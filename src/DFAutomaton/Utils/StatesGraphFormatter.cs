using PureMonads;

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
    /// <returns>A text representation of the provided state graph.</returns>
    public static string Format(State<TTransition, TState> start)
    {
        var formattedStates = new HashSet<State<TTransition, TState>>();
        var lines = GetStateLines(start, formattedStates);

        return string.Join(Environment.NewLine, lines);
    }

    private static IEnumerable<string> GetStateLines(State<TTransition, TState> state, ISet<State<TTransition, TState>> formattedStates)
    {
        if (formattedStates.Contains(state))
            yield break;

        formattedStates.Add(state);

        yield return FormatState(state);
        
        var formattedTransitions = state.Transitions.Select(stateTransition => FormatTransition(stateTransition));
        foreach (var formattedTransition in formattedTransitions)
            yield return formattedTransition;

        yield return string.Empty;

        var nextStates = state.Transitions.Select(stateTransition => stateTransition.Transition.ToState)
            .Where(nextState => nextState.HasValue)
            .Select(nextStateOption => nextStateOption.ValueOrFailure());
        var nextStatesLines = nextStates.SelectMany(nextState => GetStateLines(nextState, formattedStates));
        foreach (var nextStateLine in nextStatesLines)
            yield return nextStateLine;
    }

    private static string FormatTransition(StateTransition<TTransition, TState> stateTransition)
    {
        var (byValueOrPredicate, transition) = stateTransition;
        var (toState, _) = transition;

        return FormatTransition(byValueOrPredicate, toState);
    }

    private static string FormatState(State<TTransition, TState> state) => state.Format();

    private static string FormatTransition(
        Either<TTransition, CanTransit<TTransition>> byValueOrPredicate,
        Option<State<TTransition, TState>> toStateOption)
    {
        var toStateFormatted = toStateOption.Map(toState => toState.Id.ToString()).Or("DYNAMIC GOTO");
        var transitionFormatted = byValueOrPredicate.Match(
            byValue => byValue.ToString(),
            canTransit => canTransit.ToString());
        
        return $"    {transitionFormatted} -> State {toStateFormatted}";
    }
}