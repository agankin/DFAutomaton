namespace DFAutomaton;

/// <summary>
/// Reduces the provided automaton transition.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="automatonTransition">Automaton transition.</param>
/// <returns>Reduced state value.</returns>
public delegate TState ReduceTransition<TTransition, TState>(AutomatonTransition<TTransition, TState> automatonTransition)
    where TTransition : notnull;