namespace DFAutomaton;

/// <summary>
/// Emits a new input that will be prepended to the automaton remaining input sequence.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
/// <param name="transition">The new input.</param>
public class YieldNext<TTransition, TState>(TTransition transition)
    : ReducerEvent<TTransition, TState>
    where TTransition : notnull
{
    /// <inheritdoc/>
    internal override ReductionResult<TTransition, TState> Apply(ReductionResult<TTransition, TState> result)
    {
        return result.YieldNext(transition);
    }
}