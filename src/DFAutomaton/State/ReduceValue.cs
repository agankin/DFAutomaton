namespace DFAutomaton;

/// <summary>
/// Automaton state value reducer.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="state">Automaton state value to reduce.</param>
/// <returns>Reduced automaton state value.</returns>
public delegate TState ReduceValue<TTransition, TState>(AutomatonState<TTransition, TState> state) where TTransition : notnull;