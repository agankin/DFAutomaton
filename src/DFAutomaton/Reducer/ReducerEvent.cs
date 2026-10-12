namespace DFAutomaton;

/// <summary>
/// An event emitted by the reducer to control the behavior of the automaton.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
public abstract class ReducerEvent<TTransition, TState>
    where TTransition : notnull
{
    /// <summary>
    /// Applies the event.
    /// </summary>
    /// <param name="result">The current reduction result.</param>
    /// <returns>A new instance of <see cref="ReductionResult{TTransition, TState}"/> after the event is applied.</returns>
    internal abstract ReductionResult<TTransition, TState> Apply(ReductionResult<TTransition, TState> result);
}