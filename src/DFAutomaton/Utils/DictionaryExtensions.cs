namespace DFAutomaton;

internal static class DictionaryExtensions
{
    public static void AddValue<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict, TKey key, TValue value)
    {
        if (!dict.TryGetValue(key, out var values))
            dict[key] = values = new List<TValue>();

        values.Add(value);
    }

    public static TValue GetOr<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key, Func<TValue> getDefaultValue)
        where TKey : notnull
    {
        return dict.TryGetValue(key, out var value)
            ? value
            : getDefaultValue();
    }

    public static IReadOnlyDictionary<TKey, TValue> ToFrozen<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEqualityComparer<TKey> equalityComparer = null!)
    {
        return dict.ToDictionary(e => e.Key, e => e.Value, equalityComparer ?? EqualityComparer<TKey>.Default);
    }

    public static ILookup<TKey, TValue> ToFrozen<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict)
    {
        return dict
            .SelectMany(e => e.Value.Select(value => (e.Key, value)))
            .ToLookup(e => e.Key, e => e.value);
    }
}