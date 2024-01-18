namespace DFAutomaton;

/// <summary>
/// Automaton state value reducer.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="automatonTransition">Automaton transition to reduce.</param>
/// <returns>Reduced automaton state value.</returns>
public delegate TState ReduceValue<TTransition, TState>(AutomatonTransition<TTransition, TState> automatonTransition)
    where TTransition : notnull;