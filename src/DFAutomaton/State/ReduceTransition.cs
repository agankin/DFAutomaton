namespace DFAutomaton;

/// <summary>
/// Automaton transition reducer.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="automatonTransition">Automaton transition to reduce.</param>
/// <returns>Reduced automaton state value.</returns>
public delegate TState ReduceTransition<TTransition, TState>(AutomatonTransition<TTransition, TState> automatonTransition)
    where TTransition : notnull;