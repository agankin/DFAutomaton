using Optional;

namespace DFAutomaton;

/// <summary>
/// Reduces the provided automaton transition.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="automatonTransition">An automaton transition.</param>
/// <returns>The result of the automaton transition reduction.</returns>
public delegate ReductionResult<TTransition, TState> Reduce<TTransition, TState>(AutomatonTransition<TTransition, TState> automatonTransition)
    where TTransition : notnull;

/// <summary>
/// The result of an automaton transition reduction.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="StateValue">A new state value after the reduction performed.</param>
/// <param name="GoToState">Some next state that an automaton must transit to for dynamic transitions or None.</param>
public readonly record struct ReductionResult<TTransition, TState>(
    TState StateValue,
    Option<IState<TTransition, TState>> GoToState
)
where TTransition : notnull;