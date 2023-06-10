namespace DFAutomaton;

public record BuildConfiguration
{
    internal static readonly BuildConfiguration Default = new BuildConfiguration();

    internal bool ValidateAnyReachesAccepted { get; private init; } = true;

    public BuildConfiguration TurnOffAnyReachesAcceptedValidation() => this with { ValidateAnyReachesAccepted = false };
}