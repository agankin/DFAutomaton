namespace DFAutomaton;

/// <summary>
/// State Id.
/// </summary>
/// <param name="Value">State Id value.</param>
public readonly record struct StateId(uint Value)
{
    public static readonly StateId StartStateId = 1;
    public static readonly StateId AcceptedStateId = 0;
    public static readonly StateId SubStateStartId = 2;

    /// <summary>
    /// Returns state type.
    /// </summary>
    /// <returns>State type.</returns>
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