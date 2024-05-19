using PureMonads;

namespace DFAutomaton;

/// <summary>
/// An automaton immutable state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public readonly struct ImmutableState<TTransition, TState> where TTransition : notnull
{
    private readonly State<TTransition, TState> _state;
    
    internal ImmutableState(State<TTransition, TState> state) => _state = state;

    /// <summary>
    /// Contains an identifier that is unique within the scope of the containing state graph.
    /// </summary>
    public uint Id => _state.Id;

    /// <summary>
    /// Contains a tag with additional information.
    /// </summary>
    /// <remarks>
    /// This information will be included in the text representation of the state returned from <see cref="Format"/> method.
    /// </remarks>
    public object? Tag => _state.Tag;

    /// <summary>
    /// Contains the state type.
    /// </summary>
    public StateType Type => _state.Type;

    /// <summary>
    /// Contains transitions to next states.
    /// </summary>
    public IReadOnlyCollection<TTransition> Transitions => _state.Transitions;

    /// <summary>
    /// Returns a state transition by a transition value.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    /// <returns>Some state transition for the provided transition value or None if the transition doesn't exist.</returns>
    public Option<Transition<TTransition, TState>> this[TTransition transition] => _state[transition];
}