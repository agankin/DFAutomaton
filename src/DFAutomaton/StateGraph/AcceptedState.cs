namespace DFAutomaton;

/// <summary>
/// Automaton accepted state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class AcceptedState<TTransition, TState> where TTransition : notnull
{
    internal AcceptedState(State<TTransition, TState> state) => State = state;

    internal State<TTransition, TState> State { get; }

    /// <summary>
    /// Contains unique id within the scope of the containing state graph.
    /// </summary>
    public long Id => State.Id;

    /// <summary>
    /// Contains a tag with additional information. 
    /// </summary>
    public object? Tag
    {
        get => State.Tag;
        set => State.Tag = value;
    }
}