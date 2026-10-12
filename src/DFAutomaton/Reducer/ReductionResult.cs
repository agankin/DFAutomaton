using PureMonads;

namespace DFAutomaton;

/// <summary>
/// The state reduction result.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
internal class ReductionResult<TTransition, TState> where TTransition : notnull
{
    private readonly List<TTransition> _yieldedTransitions = [];

    public ReductionResult(TState value) => Value = value;

    /// <summary>
    /// A next state value.
    /// </summary>
    public TState Value { get; set; }

    /// <summary>
    /// Contains Some with a state the automaton will go to or None for fixed transition.
    /// </summary>
    internal Option<StateId> DynamiclyGoToStateId { get; private set; } = Option.None<StateId>();

    /// <summary>
    /// A sequence of yielded transitions.
    /// </summary>
    internal IReadOnlyCollection<TTransition> YieldedTransitions => _yieldedTransitions;

    /// <summary>
    /// Instructs the automaton to go to a state for dynamic transition.
    /// </summary>
    /// <param name="stateId">Identifier of the state.</param>
    /// <returns>The current instance of <see cref="ReductionResult{TTransition, TState}"/>.</returns>
    public ReductionResult<TTransition, TState> DynamiclyGoTo(uint stateId)
    {
        DynamiclyGoToStateId = new StateId(stateId);
        return this;
    }

    /// <summary>
    /// Adds a new input that will be prepended to the automaton remaining input sequence
    /// </summary>
    /// <param name="transition">The new input.</param>
    /// <returns>The current instance of <see cref="ReductionResult{TTransition, TState}"/>.</returns>
    public ReductionResult<TTransition, TState> YieldNext(TTransition transition)
    {
        _yieldedTransitions.Add(transition);
        return this;
    }

    public static implicit operator ReductionResult<TTransition, TState>(TState value) =>
        new ReductionResult<TTransition, TState>(value);
}