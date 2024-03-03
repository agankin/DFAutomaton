namespace DFAutomaton;

/// <summary>
/// Contains types of automaton states.
/// </summary>
public enum StateType
{
    /// <summary>
    /// The start state that an automaton execution starts at.
    /// </summary>
    Start = 1,

    /// <summary>
    /// An intermediate state at which an automaton expects next transitions.
    /// </summary>
    SubState,

    /// <summary>
    /// The accepted state at which an automaton finishes.
    /// </summary>
    Accepted
}