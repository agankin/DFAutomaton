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
    /// An attempt of transitioning from the accepted state.
    /// </summary>
    TransitionFromAccepted,

    /// <summary>
    /// No next state determined for a dynamic transition.
    /// </summary>
    NoNextState,

    /// <summary>
    /// The accepted state was not reached on the automaton finish.
    /// </summary>
    AcceptedNotReached,

    /// <summary>
    /// The automaton went to an error state.
    /// </summary>
    StateError
}