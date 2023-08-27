using DFAutomaton.Utils;

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
        var generateId = StateIdGenerator.CreateNew();
        return new State<TTransition, TState>(StateType.Start, generateId);
    }
    
    /// <summary>
    /// Creates intermediate state.
    /// </summary>
    /// <param name="generateId">State id generator.</param>
    /// <returns>Intermediate state.</returns>
    public static State<TTransition, TState> SubState(Func<long> generateId) =>
        new State<TTransition, TState>(StateType.SubState, generateId);

    /// <summary>
    /// Creates accepted state.
    /// </summary>
    /// <param name="generateId">State id generator.</param>
    /// <returns>Accepted state.</returns>
    public static State<TTransition, TState> Accepted(Func<long> generateId) =>
        new State<TTransition, TState>(StateType.Accepted, generateId);
}