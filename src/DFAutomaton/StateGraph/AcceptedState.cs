namespace DFAutomaton;

public class AcceptedState<TTransition, TState> where TTransition : notnull
{
    internal AcceptedState(State<TTransition, TState> state) => State = state;

    internal State<TTransition, TState> State { get; }

    public long Id => State.Id;

    public object? Tag
    {
        get => State.Tag;
        set => State.Tag = value;
    }
}