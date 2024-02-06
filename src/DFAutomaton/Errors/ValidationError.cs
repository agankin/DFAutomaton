namespace DFAutomaton;

/// <summary>
/// Contains types of state graph validation errors.
/// </summary>
public enum ValidationError
{
    /// <summary>
    /// The state graph contains no accepted states. 
    /// </summary>
    NoAccepted = 1,
    
    /// <summary>
    /// In the state graph no accepted state can be reached from a state.
    /// </summary>
    AcceptedIsUnreachable
}