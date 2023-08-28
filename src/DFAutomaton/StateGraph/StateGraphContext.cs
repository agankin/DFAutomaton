namespace DFAutomaton;

/// <summary>
/// State graph context data available for each state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
internal class StateGraphContext<TTransition, TState> where TTransition : notnull
{
    private long _currentId;

    public long GenerateId() => _currentId++;

    public State<TTransition, TState> AcceptedState { get; set; } = null!;

    public void SetAcceptedState(State<TTransition, TState> acceptedState) => AcceptedState = acceptedState;
}