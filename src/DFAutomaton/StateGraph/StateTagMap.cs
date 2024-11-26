namespace DFAutomaton;

internal class StateTagMap
{
    private readonly Dictionary<StateId, object> _tagByStateId = new();

    public object? this[StateId stateId]
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

    public FrozenStateTagMap ToFrozen()
    {
        var tagByStateId = _tagByStateId.ToFrozen();

        return new(tagByStateId);
    }
}