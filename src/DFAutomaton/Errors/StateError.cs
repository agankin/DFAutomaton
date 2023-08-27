namespace DFAutomaton;

/// <summary>
/// Types of errors that state validation may produce.
/// </summary>
public enum StateError
{
    /// <summary>
    /// States graph contains no accepted state. 
    /// </summary>
    NoAccepted = 1,
    
    /// <summary>
    /// Accepted state cannot be reached from a state.
    /// </summary>
    AcceptedIsUnreachable
}