using Optional;

namespace DFAutomaton;

/// <summary>
/// Automaton state reducer.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="state">Automaton state to be reduced.</param>
/// <returns>Automaton state reduction result.</returns>
public delegate ReductionResult<TTransition, TState> Reduce<TTransition, TState>(AutomatonState<TTransition, TState> state)
where TTransition : notnull;

/// <summary>
/// Automaton state reduction result.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="State">Reduced state value.</param>
/// <param name="GoToState">Some next state that automaton must go to or None.</param>
public readonly record struct ReductionResult<TTransition, TState>(
    TState State,
    Option<IState<TTransition, TState>> GoToState
)
where TTransition : notnull;