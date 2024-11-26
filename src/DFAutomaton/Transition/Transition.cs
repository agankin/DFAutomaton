using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Contains information about transition to a next state.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="ToState">Some next state for fixed transitions or None for dynamic transitions.</param>
/// <param name="Reducer">A delegate reducing state values.</param>
public readonly record struct Transition<TTransition, TState>(
    Option<State<TTransition, TState>> ToState,
    Reduce<TTransition, TState> Reducer
)
where TTransition : notnull
{
    internal TransitionEntry<TTransition, TState> ToEntry() => new TransitionEntry<TTransition, TState>(
        ToState.Map(toState => toState.Id),
        Reducer
    );
}