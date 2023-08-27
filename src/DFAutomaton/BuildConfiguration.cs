namespace DFAutomaton;

/// <summary>
/// States graph validation configuration.
/// </summary>
public record BuildConfiguration
{
    /// <summary>
    /// Default instance.
    /// </summary>
    internal static readonly BuildConfiguration Default = new();

    /// <summary>
    /// Flag to validate that any state must reach accepted. True by default.
    /// </summary>
    internal bool ValidateAnyReachesAccepted { get; private init; } = true;

    /// <summary>
    /// Turns off validation of any state must reach accepted.
    /// </summary>
    /// <returns>New instance with changes applied.</returns>
    public BuildConfiguration TurnOffAnyReachesAcceptedValidation() => this with { ValidateAnyReachesAccepted = false };
}