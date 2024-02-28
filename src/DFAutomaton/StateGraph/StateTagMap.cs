namespace DFAutomaton;

internal class StateTagMap
{
    private readonly Dictionary<uint, object> _tagByStateId = new();

    public object? this[uint stateId]
    {
        get => _tagByStateId.TryGetValue(stateId, out var tag) ? tag : null;
        set
        {
            if (value is null)
                _tagByStateId.Remove(stateId);
            else
                _tagByStateId[stateId] = value;
        }
    }
}