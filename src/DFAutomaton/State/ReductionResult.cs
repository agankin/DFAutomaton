using PureMonads;

namespace DFAutomaton;

/// <summary>
/// A state reduction result.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class ReductionResult<TTransition, TState> where TTransition : notnull
{
    private readonly List<TTransition> _yieldedTransitions = new();

    public ReductionResult(TState value) => Value = value;

    /// <summary>
    /// A next state value.
    /// </summary>
    public TState Value { get; set; }

    /// <summary>
    /// A state the automaton must go to for a dynamic transition.
    /// </summary>
    internal Option<StateId> DynamiclyGoToStateId { get; private set; } = Option.None<StateId>();

    /// <summary>
    /// Yielded next transition values.
    /// </summary>
    internal IReadOnlyCollection<TTransition> YieldedTransitions => _yieldedTransitions;

    /// <summary>
    /// Orders the automaton to go to a state.
    /// </summary>
    /// <param name="state">A state the automaton must dynamicly go to.</param>
    /// <remarks>
    /// This method calls are ignored for fixed transitions.
    /// </remarks>
    /// <returns>The current instance.</returns>
    public ReductionResult<TTransition, TState> DynamiclyGoTo(State<TTransition, TState> state)
    {
        DynamiclyGoToStateId = state.Id;
        return this;
    }

    /// <summary>
    /// Yields the provided transition value as next into automaton transitions enumeration.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    /// <remarks>
    /// The yielded value will be handled before initially provided transition values but after other previously yielded values.
    /// </remarks>
    /// <returns>The current instance.</returns>
    public ReductionResult<TTransition, TState> YieldNext(TTransition transition)
    {
        _yieldedTransitions.Add(transition);
        return this;
    }

    public static implicit operator ReductionResult<TTransition, TState>(TState value) => new(value);
}