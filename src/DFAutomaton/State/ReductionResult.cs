using PureMonads;

namespace DFAutomaton;

/// <summary>
/// A state reduction result.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
public class ReductionResult<TTransition, TState> where TTransition : notnull
{
    private readonly List<TTransition> _yieldedTransitions = new();

    public ReductionResult(TState value) => Value = value;

    /// <summary>
    /// A next state value.
    /// </summary>
    public TState Value { get; set; }

    /// <summary>
    /// Option containing a state the automaton will go to for dynamic transition or None.
    /// </summary>
    internal Option<StateId> DynamiclyGoToStateId { get; private set; } = Option.None<StateId>();

    internal IReadOnlyCollection<TTransition> YieldedTransitions => _yieldedTransitions;

    /// <summary>
    /// Orders an automaton to go to a state on dynamic transition.
    /// </summary>
    /// <param name="stateId">Id of a state the automaton will dynamicly go to on dynamic transition.</param>
    /// <remarks>
    /// The method call is ignored for fixed transitions.
    /// </remarks>
    /// <returns>The same instance of <see cref="ReductionResult{TTransition, TState}"/>.</returns>
    public ReductionResult<TTransition, TState> DynamiclyGoTo(uint stateId)
    {
        DynamiclyGoToStateId = new StateId(stateId);
        return this;
    }

    /// <summary>
    /// Yields a transition value as next into automaton transitions enumeration.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    /// <remarks>
    /// The yielded value will be handled before initially provided transition values but after other previously yielded values.
    /// </remarks>
    /// <returns>The same instance of <see cref="ReductionResult{TTransition, TState}"/>.</returns>
    public ReductionResult<TTransition, TState> YieldNext(TTransition transition)
    {
        _yieldedTransitions.Add(transition);
        return this;
    }

    public static implicit operator ReductionResult<TTransition, TState>(TState value) => new(value);
}