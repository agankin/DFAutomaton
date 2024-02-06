namespace DFAutomaton;

/// <summary>
/// Reduces the provided state value.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="stateValue">State value.</param>
/// <returns>Reduced state value.</returns>
public delegate TState ReduceValue<TTransition, TState>(TTransition transition, TState stateValue)
    where TTransition : notnull;