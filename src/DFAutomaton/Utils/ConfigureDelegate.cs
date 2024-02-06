namespace DFAutomaton.Utils;

/// <summary>
/// Invoked to change configuration.
/// </summary>
/// <typeparam name="TConfiguration">Configuration type.</typeparam>
/// <param name="configuration">Configuration.</param>
/// <returns>Changed configuration.</returns>
public delegate TConfiguration Configure<TConfiguration>(TConfiguration configuration);