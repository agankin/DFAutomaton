namespace DFAutomaton;

/// <summary>
/// Accepted state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class AcceptedState<TTransition, TState> where TTransition : notnull
{
    internal AcceptedState(State<TTransition, TState> state) => State = state;

    internal State<TTransition, TState> State { get; }

    /// <summary>
    /// State Id.
    /// </summary>
    public long Id => State.Id;

    /// <summary>
    /// Tag with some additional information attached to the state. 
    /// </summary>
    public object? Tag
    {
        get => State.Tag;
        set => State.Tag = value;
    }
}