using PureMonads;

namespace DFAutomaton;

/// <summary>
/// An automaton state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public readonly struct FrozenState<TTransition, TState> where TTransition : notnull
{
    private readonly StateId _stateId;
    private readonly FrozenStateGraph<TTransition, TState> _stateGraph;
    
    internal FrozenState(StateId stateId, FrozenStateGraph<TTransition, TState> stateGraph)
    {
        _stateId = stateId;
        _stateGraph = stateGraph;
    }

    /// <summary>
    /// Contains an identifier that is unique within the scope of the containing state graph.
    /// </summary>
    public StateId Id { get; }

    /// <summary>
    /// Contains a tag with additional information.
    /// </summary>
    /// <remarks>
    /// This information will be included in the text representation of the state returned from <see cref="Format"/> method.
    /// </remarks>
    public object? Tag => _stateGraph.GetTag(_stateId);

    /// <summary>
    /// Contains the state type.
    /// </summary>
    public StateType Type => _stateId.GetStateType();

    /// <summary>
    /// Contains transitions to next states.
    /// </summary>
    public IReadOnlyCollection<TTransition> Transitions => _stateGraph.GetTransitions(_stateId);

    /// <summary>
    /// Returns a state transition by a transition value.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    /// <returns>Some state transition for the provided transition value or None if the transition doesn't exist.</returns>
    public Option<FrozenTransition<TTransition, TState>> this[TTransition transition] => _stateGraph.GetTransition(_stateId, transition);
}