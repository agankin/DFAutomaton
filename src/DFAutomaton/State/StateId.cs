namespace DFAutomaton;

/// <summary>
/// Represents a state indentifier.
/// </summary>
/// <param name="Value">A value of the state identifier.</param>
public readonly record struct StateId(uint Value)
{
    public static readonly StateId StartStateId = 1;
    public static readonly StateId AcceptedStateId = 0;
    public static readonly StateId SubStateStartId = 2;

    /// <summary>
    /// Returns a state type corresponding to this state identifier.
    /// </summary>
    /// <returns>A <see cref="StateType"/> value.</returns>
    public StateType GetStateType()
    {
        if (Value >= SubStateStartId)
            return StateType.SubState;

        if (Value == StartStateId)
            return StateType.Start;

        return StateType.Accepted;
    }

    public static implicit operator StateId(uint id) => new(id);

    public static implicit operator uint(StateId stateId) => stateId.Value;
}