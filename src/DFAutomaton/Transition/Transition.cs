using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Contains information about transition to a next state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="ToState">Some next state for fixed transitions or None for dynamic transitions.</param>
/// <param name="Reducer">A function to reduce state value on transition.</param>
public readonly record struct Transition<TTransition, TState>(
    Option<State<TTransition, TState>> ToState,
    Reduce<TTransition, TState> Reducer
)
where TTransition : notnull;