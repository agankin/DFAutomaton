namespace DFAutomaton;

/// <summary>
/// Contains configuration for building an automaton.
/// </summary>
/// <typeparam name="TState">The state type.</typeparam>
public record AutomatonBuildConfiguration<TState>
{
    internal static readonly AutomatonBuildConfiguration<TState> Default = new();

    internal bool ValidateAnyReachesAcceptedEnabled { get; private init; }

    internal Predicate<TState>? IsErrorState { get; private init; }

    /// <summary>
    /// Enables validation of any state reaches the accepted state.
    /// </summary>
    /// <returns>A copy of the <see cref="AutomatonBuildConfiguration{TState}"/> with the setting applied.</returns>
    public AutomatonBuildConfiguration<TState> ValidateAnyCanReachAccepted() =>
        this with { ValidateAnyReachesAcceptedEnabled = true };

    /// <summary>
    /// Sets a predicate that checks an automaton state for errors.
    /// </summary>
    /// <param name="isErrorState">A predicate checking an automaton state for errors.</param>
    /// <returns>A copy of the <see cref="AutomatonBuildConfiguration{TState}"/> with the setting applied.</returns>
    public AutomatonBuildConfiguration<TState> AddCheckForErrorState(Predicate<TState> isErrorState) =>
        this with { IsErrorState = isErrorState };
}