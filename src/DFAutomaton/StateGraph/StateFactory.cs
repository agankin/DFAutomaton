namespace DFAutomaton;

/// <summary>
/// The factory to create automaton states.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
internal static class StateFactory<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// Creates a start state.
    /// </summary>
    /// <returns>Created start state.</returns>
    public static State<TTransition, TState> Start()
    {
        var graphContext = new StateGraphContext<TTransition, TState>();
        var acceptedState = Accepted(graphContext);

        graphContext.SetAcceptedState(acceptedState);

        return new State<TTransition, TState>(StateType.Start, graphContext);
    }
    
    /// <summary>
    /// Creates a new intermediate state.
    /// </summary>
    /// <param name="graphContext">State graph context.</param>
    /// <returns>Created intermediate state.</returns>
    public static State<TTransition, TState> SubState(StateGraphContext<TTransition, TState> graphContext) =>
        new State<TTransition, TState>(StateType.SubState, graphContext);

    private static State<TTransition, TState> Accepted(StateGraphContext<TTransition, TState> graphContext) =>
        new State<TTransition, TState>(StateType.Accepted, graphContext);
}