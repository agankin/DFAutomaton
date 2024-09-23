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

    public AutomatonBuilder(IEqualityComparer<TTransition>? transitionEqualityComparer = null)
    {
        _stateGraph = new(transitionEqualityComparer ?? EqualityComparer<TTransition>.Default);
        
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
        var validationResult = _configuration.ValidateAnyReachesAcceptedEnabled ? Validate(_stateGraph) : _stateGraph;

        return validationResult.Match<BuildResult<TTransition, TState>>(
            stateGraph => CreateAutomaton(stateGraph, _configuration.IsErrorState),
            error => error);
    }

    private static Automaton<TTransition, TState> CreateAutomaton(StateGraph<TTransition, TState> stateGraph, Predicate<TState>? isErrorState)
    {
        var frozenStateGraph = stateGraph.ToFrozen();
        return new Automaton<TTransition, TState>(frozenStateGraph, isErrorState);
    }

    private static ValidationResult<TTransition, TState> Validate(StateGraph<TTransition, TState> stateGraph)
    {
        Option<ValidationError> ValidateHasAccepted() => StateGraphValidator<TTransition, TState>.ValidateHasAccepted(stateGraph);
        Option<ValidationError> ValidateAnyReachAccepted() => StateGraphValidator<TTransition, TState>.ValidateAnyReachAccepted(stateGraph);

        return ValidateHasAccepted().Or(ValidateAnyReachAccepted).Match<ValidationResult<TTransition, TState>>(
            error => error,
            () => stateGraph
        );
    }
}