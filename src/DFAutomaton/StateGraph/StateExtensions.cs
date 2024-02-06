using Optional;

namespace DFAutomaton;

/// <summary>
/// Contains additional methods for state building.
/// </summary>
public static class StateExtensions
{
    /// <summary>
    /// Builds immutable state graph from the provided graph.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="start">State graph start state.</param>
    /// <param name="configuration">Validation configuration.</param>
    /// <returns>The start state of immutable state graph.</returns>
    public static Option<IState<TTransition, TState>, ValidationError> Complete<TTransition, TState>(this State<TTransition, TState> startState, ValidationConfiguration configuration)
        where TTransition : notnull
    {
        var start = startState.AsImmutable();

        return configuration.ValidateAnyReachesAccepted
            ? StateGraphValidator<TTransition, TState>.ValidateHasAccepted(start)
                .FlatMap(_ => StateGraphValidator<TTransition, TState>.ValidateAnyReachAccepted(start))
            : start.Some<IState<TTransition, TState>, ValidationError>();
    }

    /// <summary>
    /// Adds transition to a new fixed state with reducing to fixed next state value.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="nextStateValue">Next state value.</param>
    /// <returns>A new state linked by the added transition.</returns>
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
    /// Adds transition to new fixed state with reducing by the provided state value reducer.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="reduce">State value reducer.</param>
    /// <returns>A new state linked by the added transition.</returns>
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
    /// Adds transition to a new fixed state with applying the provided automaton transition reducer.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="reduce">Automaton transition reducer.</param>
    /// <returns>A new state linked by the added transition.</returns>
    public static State<TTransition, TState> ToNewFixedState<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        ReduceTransition<TTransition, TState> reduce)
        where TTransition : notnull
    {
        var newState = StateFactory<TTransition, TState>.SubState(current.GraphContext);
        return current.LinkFixedState(transition, newState, reduce);
    }

    /// <summary>
    /// Adds transition to the provided existing state with reducing to fixed next state value.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="nextState">Existing automaton state.</param>
    /// <param name="nextValue">Next state value.</param>
    /// <returns>The provided existing automaton state.</returns>
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
    /// Adds transition to the accepted state with reducing to fixed accepted state value.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="acceptedValue">Accepted state value.</param>
    /// <returns>The accepted state.</returns>
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
    /// Adds transition to the accepted state with applying state value reducer.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="reduce">State value reducer.</param>
    /// <returns>The accepted state.</returns>
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

    /// <summary>
    /// Adds transition to the accepted state with applying automaton transition reducer.
    /// </summary>
    /// <typeparam name="TTransition">Transition value type.</typeparam>
    /// <typeparam name="TState">State value type.</typeparam>
    /// <param name="current">Current state.</param>
    /// <param name="transition">Transition value.</param>
    /// <param name="reduce">Automaton transition reducer.</param>
    /// <returns>The accepted state.</returns>
    public static AcceptedState<TTransition, TState> ToAccepted<TTransition, TState>(
        this State<TTransition, TState> current,
        TTransition transition,
        ReduceTransition<TTransition, TState> reduce)
        where TTransition : notnull
    {
        var acceptedState = current.GraphContext.AcceptedState;
        current.LinkFixedState(transition, acceptedState, reduce);

        return new AcceptedState<TTransition, TState>(acceptedState);
    }
    
    private static ReduceTransition<TTransition, TState> Constant<TTransition, TState>(TState newValue)
        where TTransition : notnull
    {
        return _ => newValue;
    }

    private static IState<TTransition, TState> AsImmutable<TTransition, TState>(this State<TTransition, TState> current)
        where TTransition : notnull
    {
        return current;
    }
}