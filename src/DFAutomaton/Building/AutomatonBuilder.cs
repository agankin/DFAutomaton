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
        var stateGraph = new StateGraph<TTransition, TState>();
        var start = stateGraph.StartState;
        
        return new AutomatonBuilder<TTransition, TState>(start);
    }

    /// <summary>
    /// Builds a new automaton.
    /// </summary>
    /// <param name="configure">A delegate to setup the build configuration.</param>
    /// <returns>The result of the build.</returns>
    public BuildResult<TTransition, TState> Build(Configure<AutomatonBuildConfiguration>? configure = null)
    {
        var configuration = (configure ?? (config => config))(AutomatonBuildConfiguration.Default);
        var result = Validate(Start, configuration);

        return result.Value.Match<BuildResult<TTransition, TState>>(
            startState => new Automaton<TTransition, TState>(startState),
            error => error);
    }

    private static ValidationResult<TTransition, TState> Validate(State<TTransition, TState> startState, AutomatonBuildConfiguration configuration)
    {
        if (configuration.ValidateAnyReachesAcceptedEnabled)
        {
            return StateGraphValidator<TTransition, TState>.ValidateHasAccepted(startState).Value
                .FlatMap(_ => StateGraphValidator<TTransition, TState>.ValidateAnyReachAccepted(startState).Value);
        }

        return startState.Some<State<TTransition, TState>, ValidationError>();
    }
}