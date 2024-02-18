using DFAutomaton.Utils;
using Optional;

namespace DFAutomaton;

/// <summary>
/// Builder for building automatons.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class AutomatonBuilder<TTransition, TState> where TTransition : notnull
{
    private AutomatonBuilder(State<TTransition, TState> start) => Start = start;

    /// <summary>
    /// The start state.
    /// </summary>
    public State<TTransition, TState> Start { get; }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <returns>The created new instance.</returns> 
    public static AutomatonBuilder<TTransition, TState> Create()
    {
        var start = StateFactory<TTransition, TState>.Start();
        return new AutomatonBuilder<TTransition, TState>(start);
    }

    /// <summary>
    /// Builds a new automaton.
    /// </summary>
    /// <param name="configure">The delegate to setup a build configuration.</param>
    /// <returns>Some with a new automaton or None with an error.</returns>
    public Option<Automaton<TTransition, TState>, ValidationError> Build(Configure<ValidationConfiguration>? configure = null)
    {
        var configuration = (configure ?? (config => config))(ValidationConfiguration.Default);
        var startOrError = Start.Build(configuration);

        return startOrError.Map<Automaton<TTransition, TState>>(start => new(start));
    }
}