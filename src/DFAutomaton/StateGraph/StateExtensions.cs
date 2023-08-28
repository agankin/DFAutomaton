using Optional;

namespace DFAutomaton;

/// <summary>
/// Extensions that provide operations for state building.
/// </summary>
public static class StateExtensions
{
    /// <summary>
    /// Completes states graph building returning start state of immutable states graph.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="start">States graph start.</param>
    /// <param name="configuration">Validation configuration.</param>
    /// <returns>Start state of immutable states graph.</returns>
    public static Option<IState<TTransition, TState>, StateError> Complete<TTransition, TState>(this State<TTransition, TState> start, ValidationConfiguration configuration)
        where TTransition : notnull
    {
        return configuration.ValidateAnyReachesAccepted
            ? start.AsImmutable().ValidateAnyReachAccepted()
            : start.AsImmutable().Some<IState<TTransition, TState>, StateError>();
    }

    /// <summary>
    /// Adds transition to new fixed state with reducing to fixed value.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="nextStateValue">Next state value.</param>
    /// <returns>Next state.</returns>
    public static State<TTransition, TState> ToNewFixedState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        TState nextStateValue)
        where TTransition : notnull
    {
        var reduce = Constant<TTransition, TState>(nextStateValue);
        return current.ToNewFixedState(transition, reduce);
    }

    /// <summary>
    /// Adds transition to new fixed state with applying value reducer.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="reduce">State value reducer.</param>
    /// <returns>Next state.</returns>
    public static State<TTransition, TState> ToNewFixedState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        ReduceValue<TTransition, TState> reduce)
        where TTransition : notnull
    {
        var newState = StateFactory<TTransition, TState>.SubState(current.GraphContext);
        return current.LinkFixedState(transition, newState, reduce);
    }

    /// <summary>
    /// Adds transition to existing automaton state with reducing to fixed value.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="nextState">Next automaton state.</param>
    /// <param name="nextValue">Next state value.</param>
    /// <returns>Next state.</returns>
    public static State<TTransition, TState> LinkFixedState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        State<TTransition, TState> nextState,
        TState nextValue)
        where TTransition : notnull
    {
        var reduce = Constant<TTransition, TState>(nextValue);
        return current.LinkFixedState(transition, nextState, reduce);
    }

    /// <summary>
    /// Adds transition to the accepted state with reducing to fixed value.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="acceptedValue">Next accepted state value.</param>
    /// <returns>Accepted state.</returns>
    public static AcceptedState<TTransition, TState> ToAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        TState acceptedValue)
        where TTransition : notnull
    {
        var reduce = Constant<TTransition, TState>(acceptedValue);
        return current.ToAccepted(transition, reduce);
    }

    /// <summary>
    /// Adds transition to the accepted state with applying value reducer.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="reduce">State value reducer.</param>
    /// <returns>Accepted state.</returns>
    public static AcceptedState<TTransition, TState> ToAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        ReduceValue<TTransition, TState> reduce)
        where TTransition : notnull
    {
        var acceptedState = current.GraphContext.AcceptedState;
        current.LinkFixedState(transition, acceptedState, reduce);

        return new AcceptedState<TTransition, TState>(acceptedState);
    }
    
    private static ReduceValue<TTransition, TState> Constant<TTransition, TState>(TState newValue)
        where TTransition : notnull =>
        _ => newValue;

    private static IState<TTransition, TState> AsImmutable<TTransition, TState>(this State<TTransition, TState> current)
        where TTransition : notnull
        => current;
}