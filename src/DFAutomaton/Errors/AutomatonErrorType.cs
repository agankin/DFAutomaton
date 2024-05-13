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
    /// No next state for a dynamic transition.
    /// </summary>
    NoNextState,

    /// <summary>
    /// The accepted state was not reached after the automaton finished.
    /// </summary>
    AcceptedNotReached,

    /// <summary>
    /// An error state returned from a reducer.
    /// </summary>
    ReducerError
}