namespace DFAutomaton;

/// <summary>
/// Represents an accepted state.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
public readonly struct AcceptedState<TTransition, TState> where TTransition : notnull
{
    private readonly StateId _id;
    private readonly StateGraph<TTransition, TState> _owningGraph;

    internal AcceptedState(StateId id, StateGraph<TTransition, TState> owningGraph)
    {
        _id = id;
        _owningGraph = owningGraph;
    }

    /// <summary>
    /// Contains an identifier that is unique within the scope of the containing state graph.
    /// </summary>
    public StateId Id => _id;

    /// <summary>
    /// Contains a tag with additional information.
    /// </summary>
    /// <remarks>
    /// This information will be included in the text representation of the state returned from <see cref="Format"/> method.
    /// </remarks>
    public object? Tag
    {
        get => _owningGraph.GetTag(_id);
        set => _owningGraph.SetTag(_id, value);
    }

    /// <summary>
    /// Returns a text representation of the state.
    /// </summary>
    public string Format() => string.IsNullOrEmpty(Tag?.ToString())
        ? $"State {Id}"
        : $"State {Id}: {Tag}";
}