using PureMonads;

namespace DFAutomaton;

/// <summary>
/// A builder for building automata.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class AutomatonBuilder<TTransition, TState> where TTransition : notnull
{
    private readonly StateGraph<TTransition, TState> _stateGraph;
    private AutomatonBuildConfiguration<TState> _configuration = AutomatonBuildConfiguration<TState>.Default;

    private AutomatonBuilder(StateGraph<TTransition, TState> stateGraph)
    {
        _stateGraph = stateGraph;
        
        Start = _stateGraph.StartState;
        Accepted = _stateGraph.AcceptedState;
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
    public State<TTransition, TState> CreateState() => _stateGraph.CreateState();

    /// <summary>
    /// Enables validation of any state reaches the accepted state.
    /// </summary>
    /// <returns>The same instance of the builder.</returns>
    public AutomatonBuilder<TTransition, TState> ValidateAnyCanReachAccepted()
    {
        _configuration = _configuration.ValidateAnyCanReachAccepted();
        return this;
    }

    /// <summary>
    /// Sets a predicate for checking is automaton state an error state.
    /// </summary>
    /// <param name="isErrorState">A predicate for checking is automaton state an error state.</param>
    /// <returns>The same instance of the builder.</returns>
    public AutomatonBuilder<TTransition, TState> AddCheckForErrorState(Predicate<TState> isErrorState)
    {
        _configuration = _configuration.AddCheckForErrorState(isErrorState);
        return this;
    }

    /// <summary>
    /// Builds a new automaton.
    /// </summary>
    /// <returns>The result of the build.</returns>
    public BuildResult<TTransition, TState> Build()
    {
        var result = Validate();

        return result.Value
            .Map(stateGraph => stateGraph.ToFrozen())
            .Match<BuildResult<TTransition, TState>>(
                stateGraph => new Automaton<TTransition, TState>(stateGraph, _configuration.IsErrorState),
                error => error);
    }

    private ValidationResult<TTransition, TState> Validate()
    {
        if (_configuration.ValidateAnyReachesAcceptedEnabled)
        {
            return StateGraphValidator<TTransition, TState>.ValidateHasAccepted(_stateGraph).Value
                .FlatMap(_ => StateGraphValidator<TTransition, TState>.ValidateAnyReachAccepted(_stateGraph).Value);
        }

        return _stateGraph;
    }
}