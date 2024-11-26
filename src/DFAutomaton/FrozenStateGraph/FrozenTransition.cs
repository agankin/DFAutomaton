using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Contains information about transition to a next state.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="ToState">Some next state for fixed transitions or None for dynamic transitions.</param>
/// <param name="Reducer">A function to reduce state value on transition.</param>
public readonly record struct FrozenTransition<TTransition, TState>(
    Option<FrozenState<TTransition, TState>> ToState,
    Reduce<TTransition, TState> Reducer
)
where TTransition : notnull;