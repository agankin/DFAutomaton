using DFAutomaton.Utils;
using Optional;

namespace DFAutomaton;

/// <summary>
/// A builder for building automata.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class AutomatonBuilder<TTransition, TState> where TTransition : notnull
{
    private readonly StateGraph<TTransition, TState> _graph;

    private AutomatonBuilder(StateGraph<TTransition, TState> graph)
    {
        _graph = graph;
        
        Start = _graph.StartState;
        Accepted = _graph.AcceptedState;
    }

    /// <summary>
    /// The start state.
    /// </summary>
    public State<TTransition, TState> Start { get; }

    /// <summary>
    /// The accepted state.
    /// </summary>
    public State<TTransition, TState> Accepted { get; }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <returns>The created new instance.</returns> 
    public static AutomatonBuilder<TTransition, TState> Create()
    {
        var graph = new StateGraph<TTransition, TState>();
        
        return new AutomatonBuilder<TTransition, TState>(graph);
    }

    /// <summary>
    /// Creates a new state.
    /// </summary>
    /// <returns>The created new state.</returns>
    public State<TTransition, TState> CreateState() => _graph.CreateState();

    /// <summary>
    /// Builds a new automaton.
    /// </summary>
    /// <param name="configure">A delegate to setup the build configuration.</param>
    /// <returns>The result of the build.</returns>
    public BuildResult<TTransition, TState> Build(Configure<AutomatonBuildConfiguration<TState>>? configure = null)
    {
        var configuration = (configure ?? (config => config))(AutomatonBuildConfiguration<TState>.Default);
        var result = Validate(Start, configuration);

        return result.Value.Match<BuildResult<TTransition, TState>>(
            startState => new Automaton<TTransition, TState>(startState, configuration.IsErrorState),
            error => error);
    }

    private static ValidationResult<TTransition, TState> Validate(
        State<TTransition, TState> startState,
        AutomatonBuildConfiguration<TState> configuration)
    {
        if (configuration.ValidateAnyReachesAcceptedEnabled)
        {
            return StateGraphValidator<TTransition, TState>.ValidateHasAccepted(startState).Value
                .FlatMap(_ => StateGraphValidator<TTransition, TState>.ValidateAnyReachAccepted(startState).Value);
        }

        return startState.Some<State<TTransition, TState>, ValidationError>();
    }
}