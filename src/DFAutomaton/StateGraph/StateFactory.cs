namespace DFAutomaton;

/// <summary>
/// State factory.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
internal static class StateFactory<TTransition, TState> where TTransition : notnull
{
    /// <summary>
    /// Creates start state.
    /// </summary>
    /// <returns>Start state.</returns>
    public static State<TTransition, TState> Start()
    {
        var graphContext = new StateGraphContext<TTransition, TState>();
        var acceptedState = Accepted(graphContext);

        graphContext.SetAcceptedState(acceptedState);

        return new State<TTransition, TState>(StateType.Start, graphContext);
    }
    
    /// <summary>
    /// Creates intermediate state.
    /// </summary>
    /// <param name="graphContext">State graph context.</param>
    /// <returns>Intermediate state.</returns>
    public static State<TTransition, TState> SubState(StateGraphContext<TTransition, TState> graphContext) =>
        new State<TTransition, TState>(StateType.SubState, graphContext);

    private static State<TTransition, TState> Accepted(StateGraphContext<TTransition, TState> graphContext) =>
        new State<TTransition, TState>(StateType.Accepted, graphContext);
}