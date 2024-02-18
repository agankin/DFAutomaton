using DFAutomaton.Utils;

namespace DFAutomaton;

internal class StateGraph<TTransition, TState> where TTransition : notnull
{
    private Func<long> _generateNextId = SequentialIdGenerator.CreateNew();

    public State<TTransition, TState> StartState { get; private set; } = null!;
    
    public State<TTransition, TState> AcceptedState { get; private set; } = null!;
    
    public void Initialize(State<TTransition, TState> startState, State<TTransition, TState> acceptedState)
    {
        StartState = startState;
        AcceptedState = acceptedState;
    }

    public long GenerateNextId() => _generateNextId();
}