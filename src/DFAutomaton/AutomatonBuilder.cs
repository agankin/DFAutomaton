using Optional;

namespace DFAutomaton;

/// <summary>
/// Automaton builder.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class AutomatonBuilder<TTransition, TState> where TTransition : notnull
{
    private AutomatonBuilder(State<TTransition, TState> start) => Start = start;

    /// <summary>
    /// Start state.
    /// </summary>
    public State<TTransition, TState> Start { get; }

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <returns>New instance.</returns> 
    public static AutomatonBuilder<TTransition, TState> Create()
    {
        var start = StateFactory<TTransition, TState>.Start();
        return new AutomatonBuilder<TTransition, TState>(start);
    }

    /// <summary>
    /// Builds automaton.
    /// </summary>
    /// <param name="configure">Validation configuration function.</param>
    /// <returns>Automaton or error.</returns>
    public Option<Automaton<TTransition, TState>, StateError> Build(Func<BuildConfiguration, BuildConfiguration>? configure = null)
    {
        var configuration = (configure ?? (config => config))(BuildConfiguration.Default);
        var startOrError = Start.Complete(configuration);

        return startOrError.Map<Automaton<TTransition, TState>>(start => new(start));
    }
}