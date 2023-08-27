namespace DFAutomaton;

/// <summary>
/// Automaton state type.
/// </summary>
public enum StateType
{
    /// <summary>
    /// Start state automaton execution starts from.
    /// </summary>
    Start = 1,

    /// <summary>
    /// Intermediate state after that automaton must continue on.
    /// </summary>
    SubState,

    /// <summary>
    /// Accepted state that automaton must finish on.
    /// </summary>
    Accepted
}