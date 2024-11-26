using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Configures and builds an automaton.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
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
    /// <returns>A new instance of <see cref="State{TTransition, TState}"/>.</returns>
    public State<TTransition, TState> CreateState() => _stateGraph.CreateState();

    /// <summary>
    /// Enables validation of any state reaches the accepted state.
    /// </summary>
    /// <returns>The same instance of <see cref="AutomatonBuilder{TTransition, TState}"/>.</returns>
    public AutomatonBuilder<TTransition, TState> ValidateAnyCanReachAccepted()
    {
        _configuration = _configuration.ValidateAnyCanReachAccepted();
        return this;
    }

    /// <summary>
    /// Sets a predicate that checks an automaton state for errors.
    /// </summary>
    /// <param name="isErrorState">A predicate for checking an automaton state for errors.</param>
    /// <returns>The same instance of <see cref="AutomatonBuilder{TTransition, TState}"/>.</returns>
    public AutomatonBuilder<TTransition, TState> AddCheckForErrorState(Predicate<TState> isErrorState)
    {
        _configuration = _configuration.AddCheckForErrorState(isErrorState);
        return this;
    }

    /// <summary>
    /// Builds a new instance of automaton.
    /// </summary>
    /// <returns>An instance of <see cref="BuildResult{TTransition, TState}"/>.</returns>
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

        return ValidateHasAccepted()
            .Or(ValidateAnyReachAccepted)
            .Match<ValidationResult<TTransition, TState>>(error => error, () => stateGraph);
    }
}