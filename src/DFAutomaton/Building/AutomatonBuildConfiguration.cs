namespace DFAutomaton;

/// <summary>
/// Contains an automaton build configuration.
/// </summary>
/// <typeparam name="TState">State value type.</typeparam>
public record AutomatonBuildConfiguration<TState>
{
    /// <summary>
    /// The default instance.
    /// </summary>
    internal static readonly AutomatonBuildConfiguration<TState> Default = new();

    /// <summary>
    /// Contains a flag to validate that any state reaches the accepted state. False by default.
    /// </summary>
    internal bool ValidateAnyReachesAcceptedEnabled { get; private init; }

    /// <summary>
    /// A predicate for checking is automaton state an error state.
    /// </summary>
    internal Predicate<TState>? IsErrorState { get; private init; }

    /// <summary>
    /// Enables validation of any state reaches the accepted state.
    /// </summary>
    /// <returns>A new instance with the changes applied.</returns>
    public AutomatonBuildConfiguration<TState> ValidateAnyCanReachAccepted() =>
        this with { ValidateAnyReachesAcceptedEnabled = true };

    /// <summary>
    /// Sets a predicate for checking is automaton state an error state.
    /// </summary>
    /// <param name="isErrorState">A predicate for checking is automaton state an error state.</param>
    /// <returns>A new instance with the changes applied.</returns>
    public AutomatonBuildConfiguration<TState> AddCheckForErrorState(Predicate<TState> isErrorState) =>
        this with { IsErrorState = isErrorState };
}