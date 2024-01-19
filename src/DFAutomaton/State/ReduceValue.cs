namespace DFAutomaton;

/// <summary>
/// Automaton value reducer.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="stateValue">Automaton state value to reduce.</param>
/// <returns>Reduced automaton state value.</returns>
public delegate TState ReduceValue<TTransition, TState>(TTransition transition, TState stateValue)
    where TTransition : notnull;