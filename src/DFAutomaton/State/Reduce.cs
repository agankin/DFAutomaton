namespace DFAutomaton;

/// <summary>
/// Reduces the provided previous state and transition and returns a next state value.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="state">A state value.</param>
/// <param name="transition">A transition value.</param>
/// <returns>A next state value.</returns>
public delegate TState Reduce<TTransition, TState>(TState state, TTransition transition) where TTransition : notnull;