namespace DFAutomaton;

internal class FrozenStateTagMap
{
    private readonly IReadOnlyDictionary<StateId, object> _tagByStateId;

    public FrozenStateTagMap(IReadOnlyDictionary<StateId, object> tagByStateId) => _tagByStateId = tagByStateId;

    public object? this[StateId stateId] => _tagByStateId.TryGetValue(stateId, out var tag) ? tag : null;
}