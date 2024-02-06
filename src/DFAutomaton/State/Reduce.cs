using Optional;

namespace DFAutomaton;

/// <summary>
/// Reduces the provided automaton transition.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="automatonTransition">Automaton transition.</param>
/// <returns>Result of the automaton transition reduction.</returns>
public delegate ReductionResult<TTransition, TState> Reduce<TTransition, TState>(AutomatonTransition<TTransition, TState> automatonTransition)
    where TTransition : notnull;

/// <summary>
/// Result of reduction of an automaton transition.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="State">A new state value after transition.</param>
/// <param name="GoToState">Some next state that an automaton must transit to for dynamic transitions or None.</param>
public readonly record struct ReductionResult<TTransition, TState>(
    TState StateValue,
    Option<IState<TTransition, TState>> GoToState
)
where TTransition : notnull;