namespace DFAutomaton;

/// <summary>
/// Contains configuration for state graphs validation.
/// </summary>
public record ValidationConfiguration
{
    /// <summary>
    /// Default instance.
    /// </summary>
    internal static readonly ValidationConfiguration Default = new();

    /// <summary>
    /// Flag to validate that any state must reach accepted. True by default.
    /// </summary>
    internal bool ValidateAnyReachesAccepted { get; private init; } = true;

    /// <summary>
    /// Turns off validation of any state in a graph must reach accepted.
    /// </summary>
    /// <returns>New configuration with turned off validation of any state in a graph must reach accepted.</returns>
    public ValidationConfiguration TurnOffAnyReachesAcceptedValidation() => this with { ValidateAnyReachesAccepted = false };
}