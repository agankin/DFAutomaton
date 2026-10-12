namespace DFAutomaton;

/// <summary>
/// A delegate reducing state values.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="state">A state value.</param>
/// <param name="transition">A transition value.</param>
/// <param name="emit">An action to emit an event to control the behavior of the automaton.</param>
/// <returns>The new state value.</returns>
public delegate TState Reduce<TTransition, TState>(
    TState state,
    TTransition transition,
    Action<ReducerEvent<TTransition, TState>> emit
) where TTransition : notnull;