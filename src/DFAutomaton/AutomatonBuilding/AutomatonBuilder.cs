using Optional;

namespace DFAutomaton;

public class AutomatonBuilder<TTransition, TState> where TTransition : notnull
{
    private AutomatonBuilder(State<TTransition, TState> startState) => StartState = startState;

    public State<TTransition, TState> StartState { get; }

    public static AutomatonBuilder<TTransition, TState> Create()
    {
        var startState = StateFactory<TTransition, TState>.Start();
        return new AutomatonBuilder<TTransition, TState>(startState);
    }

    public Option<Automaton<TTransition, TState>, AutomatonGraphError> Build(Func<BuildConfiguration, BuildConfiguration>? configure = null)
    {
        var configuration = (configure ?? (config => config))(BuildConfiguration.Default);
        var startOrError = StartState.BuildAutomatonGraph(configuration);

        return startOrError.Map<Automaton<TTransition, TState>>(start => new(start));
    }
}