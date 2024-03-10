namespace DFAutomaton;

/// <summary>
/// Contains an automaton build configuration.
/// </summary>
public record AutomatonBuildConfiguration
{
    /// <summary>
    /// The default instance.
    /// </summary>
    internal static readonly AutomatonBuildConfiguration Default = new();

    /// <summary>
    /// Contains a flag to validate that any state reaches the accepted state. False by default.
    /// </summary>
    internal bool ValidateAnyReachesAcceptedEnabled { get; private init; }

    /// <summary>
    /// Enables validation of any state reaches the accepted state.
    /// </summary>
    /// <returns>A new instance with the changes applied.</returns>
    public AutomatonBuildConfiguration ValidateAnyReachesAccepted() =>
        this with { ValidateAnyReachesAcceptedEnabled = true };
}