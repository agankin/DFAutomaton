namespace DFAutomaton;

/// <summary>
/// Contains types of state graph validation errors.
/// </summary>
public enum ValidationError
{
    /// <summary>
    /// The state graph contains no accepted state. 
    /// </summary>
    NoAccepted = 1,
    
    /// <summary>
    /// In the state graph the accepted state cannot be reached from a state.
    /// </summary>
    AcceptedIsUnreachable
}