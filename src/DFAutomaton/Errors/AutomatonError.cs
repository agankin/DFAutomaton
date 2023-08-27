namespace DFAutomaton;

/// <summary>
/// Automaton execution error.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="Type">Error type.</param>
/// <param name="State">State the error occured at.</param>
/// <param name="Transition">Transition the error occured during.</param>
public record AutomatonError<TTransition, TState>(
    AutomatonErrorType Type,
    IState<TTransition, TState> State,
    TTransition Transition
)
where TTransition : notnull;

/// <summary>
/// Types of errors that an automaton may produce.
/// </summary>
public enum AutomatonErrorType
{
    /// <summary>
    /// Transition is not found.
    /// </summary>
    TransitionNotFound = 1,
    
    /// <summary>
    /// Attempt of transferring from an accepted state.
    /// </summary>
    TransitionFromAccepted
}