namespace DFAutomaton;

/// <summary>
/// Contains configuration for a state graph validation.
/// </summary>
public record ValidationConfiguration
{
    /// <summary>
    /// The default instance.
    /// </summary>
    internal static readonly ValidationConfiguration Default = new();

    /// <summary>
    /// Contains a flag to validate that any state reaches the accepted state. True by default.
    /// </summary>
    internal bool ValidateAnyReachesAccepted { get; private init; } = true;

    /// <summary>
    /// Turns off validation of any state reaches the accepted state.
    /// </summary>
    /// <returns>A new instance with the changes applied.</returns>
    public ValidationConfiguration TurnOffAnyReachesAcceptedValidation() => this with { ValidateAnyReachesAccepted = false };
}