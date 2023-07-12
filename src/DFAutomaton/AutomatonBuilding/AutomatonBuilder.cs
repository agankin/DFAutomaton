using Optional;

namespace DFAutomaton;

public class AutomatonBuilder<TTransition, TState> where TTransition : notnull
{
    private AutomatonBuilder(State<TTransition, TState> start) => Start = start;

    public State<TTransition, TState> Start { get; }

    public static AutomatonBuilder<TTransition, TState> Create()
    {
        var start = StateFactory<TTransition, TState>.Start();
        return new AutomatonBuilder<TTransition, TState>(start);
    }

    public Option<Automaton<TTransition, TState>, StateError> Build(Func<BuildConfiguration, BuildConfiguration>? configure = null)
    {
        var configuration = (configure ?? (config => config))(BuildConfiguration.Default);
        var startOrError = Start.Complete(configuration);

        return startOrError.Map<Automaton<TTransition, TState>>(start => new(start));
    }
}