namespace DFAutomaton;

/// <summary>
/// Contains types of automaton states.
/// </summary>
public enum StateType
{
    /// <summary>
    /// The start state an automaton starts from.
    /// </summary>
    Start = 1,

    /// <summary>
    /// An intermediate state an automaton is not expected to finish at.
    /// </summary>
    SubState,

    /// <summary>
    /// The accepted state an automaton finishes on.
    /// </summary>
    Accepted
}