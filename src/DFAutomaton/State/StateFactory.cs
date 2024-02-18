namespace DFAutomaton;

internal static class StateFactory<TTransition, TState> where TTransition : notnull
{
    public static State<TTransition, TState> Start()
    {
        var graph = new StateGraph<TTransition, TState>();
        
        var startState = new State<TTransition, TState>(StateType.Start, graph);
        var acceptedState = new State<TTransition, TState>(StateType.Accepted, graph);

        graph.Initialize(startState, acceptedState);

        return startState;
    }
    
    public static State<TTransition, TState> SubState(StateGraph<TTransition, TState> graph) =>
        new State<TTransition, TState>(StateType.SubState, graph);
}