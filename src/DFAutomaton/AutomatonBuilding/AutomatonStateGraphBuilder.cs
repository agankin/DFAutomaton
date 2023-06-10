using Optional;
using Optional.Collections;

namespace DFAutomaton;

internal static class AutomatonStateGraphBuilder
{
    public static Option<IState<TTransition, TState>, AutomatonGraphError> BuildAutomatonGraph<TTransition, TState>(
        this State<TTransition, TState> start,
        BuildConfiguration configuration)
        where TTransition : notnull
    {
        var buildedStates = new Dictionary<State<TTransition, TState>, AutomatonState<TTransition, TState>>();
        var startState = start.ToAutomatonState(buildedStates).Some<IState<TTransition, TState>, AutomatonGraphError>();
        
        return configuration.ValidateAnyReachesAccepted
            ? startState.FlatMap(AutomatonStateGraphValidator<TTransition, TState>.ValidateAnyReachAccepted)
            : startState;
    }

    private static IState<TTransition, TState> ToAutomatonState<TTransition, TState>(
        this State<TTransition, TState> state,
        IDictionary<State<TTransition, TState>, AutomatonState<TTransition, TState>> buildedStates)
        where TTransition : notnull
    {
        var type = state.Type;
        var automatonNextStates = new StateTransitionDict<TTransition, TState>();
        var automatonState = buildedStates[state] = new(state.Id, state.Tag, type, automatonNextStates);

        state.GetTransitions()
            .ToAutomatonTransitions(buildedStates)
            .CopyTo(automatonNextStates);

        return automatonState;
    }

    private static IStateTransitionDict<TTransition, TState> ToAutomatonTransitions<TTransition, TState>(
        this ITransitionDict<TTransition, TState> nextStates,
        IDictionary<State<TTransition, TState>, AutomatonState<TTransition, TState>> buildedStates)
        where TTransition : notnull
    {
        var dictionary = nextStates.ToDictionary(
            nextState => nextState.Key,
            nextState => nextState.Value.ToAutomatonTransition(buildedStates));

        return new StateTransitionDict<TTransition, TState>(dictionary);
    }

    private static StateTransition<TTransition, TState> ToAutomatonTransition<TTransition, TState>(
        this Transition<TTransition, TState> nextState,
        IDictionary<State<TTransition, TState>, AutomatonState<TTransition, TState>> buildedStates)
        where TTransition : notnull
    {
        var (state, reducer) = nextState;
        var automatonState = buildedStates.GetValueOrNone(state)
            .Match(
                automatonState => automatonState,
                () => state.ToAutomatonState(buildedStates));

        return new StateTransition<TTransition, TState>(automatonState, reducer);
    }

    private static void CopyTo<TTransition, TState>(
        this IReadOnlyDictionary<TTransition, StateTransition<TTransition, TState>> sourceDict,
        IDictionary<TTransition, StateTransition<TTransition, TState>> destDict)
        where TTransition : notnull
    {
        foreach (var keyValue in sourceDict)
            destDict[keyValue.Key] = keyValue.Value;
    }
}