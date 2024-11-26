namespace DFAutomaton;

/// <summary>
/// A delegate reducing state values.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="state">A state value.</param>
/// <param name="transition">A transition value.</param>
/// <returns>An instance of <see cref="ReductionResult{TTransition, TState}"/>.</returns>
public delegate ReductionResult<TTransition, TState> Reduce<TTransition, TState>(
    TState state,
    TTransition transition
) where TTransition : notnull;