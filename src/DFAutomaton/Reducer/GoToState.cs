namespace DFAutomaton;

/// <summary>
/// Instructs the automaton to go to a state.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="stateId">Identifier of the state.</param>
public class GoToState<TTransition, TState>(uint stateId)
    : ReducerEvent<TTransition, TState>
    where TTransition : notnull
{
    /// <inheritdoc/>
    internal override ReductionResult<TTransition, TState> Apply(ReductionResult<TTransition, TState> result)
    {
        return result.DynamiclyGoTo(stateId);
    }
}