namespace DFAutomaton;

/// <summary>
/// Contains information about an automaton runtime error.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="Type">Error type.</param>
/// <param name="State">An automaton state the error occured when transitioning from.</param>
/// <param name="Transition">A transition caused the error.</param>
public record AutomatonError<TTransition, TState>(
    AutomatonErrorType Type,
    IState<TTransition, TState> State,
    TTransition Transition
)
where TTransition : notnull;

/// <summary>
/// Contains types of automaton runtime errors.
/// </summary>
public enum AutomatonErrorType
{
    /// <summary>
    /// A transition from a state does not exist.
    /// </summary>
    TransitionNotFound = 1,
    
    /// <summary>
    /// An attempt of performing a transition from an accepted state.
    /// </summary>
    TransitionFromAccepted
}