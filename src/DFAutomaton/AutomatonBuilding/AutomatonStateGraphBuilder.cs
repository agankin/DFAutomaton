using Optional;

namespace DFAutomaton;

internal static class AutomatonStateGraphBuilder
{
    public static Option<IState<TTransition, TState>, AutomatonGraphError> Complete<TTransition, TState>(
        this State<TTransition, TState> start,
        BuildConfiguration configuration)
        where TTransition : notnull
    {
        var startState = start.AsImmutable();
        
        return configuration.ValidateAnyReachesAccepted
            ? startState.ValidateAnyReachAccepted()
            : startState.Some<IState<TTransition, TState>, AutomatonGraphError>();
    }
}