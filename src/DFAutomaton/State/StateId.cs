namespace DFAutomaton;

internal readonly record struct StateId(uint Value)
{
    public const uint StartStateId = 1;
    public const uint AcceptedStateId = 0;
    public const uint SubStateStartId = 2;

    public StateType GetStateType() => Value switch
    {
        StartStateId => StateType.Start,
        AcceptedStateId => StateType.Accepted,
        _ => StateType.SubState
    };

    public static implicit operator StateId(uint id) => new(id);

    public static implicit operator uint(StateId stateId) => stateId.Value;
}