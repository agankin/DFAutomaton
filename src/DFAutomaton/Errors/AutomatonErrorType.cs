namespace DFAutomaton;

/// <summary>
/// Contains types of automaton runtime errors.
/// </summary>
public enum AutomatonErrorType
{
    /// <summary>
    /// A transition from a state does not exist.
    /// </summary>
    TransitionNotExists = 1,
    
    /// <summary>
    /// An attempt of performing transition from the accepted state.
    /// </summary>
    TransitionFromAccepted,

    /// <summary>
    /// The accepted state was not reached after the automaton finished.
    /// </summary>
    AcceptedNotReached,

    /// <summary>
    /// An error occured on a reducer invocation.
    /// </summary>
    ReducerError,

    /// <summary>
    /// An error occured on the automaton run.
    /// </summary>
    RunError
}