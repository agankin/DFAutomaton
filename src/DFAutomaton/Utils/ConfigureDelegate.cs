namespace DFAutomaton.Utils;

/// <summary>
/// The delegate to be invoked for changing configuration values.
/// </summary>
/// <typeparam name="TConfiguration">Configuration type.</typeparam>
/// <param name="configuration">A configuration initial value.</param>
/// <returns>The configuration value after changes made.</returns>
public delegate TConfiguration Configure<TConfiguration>(TConfiguration configuration);