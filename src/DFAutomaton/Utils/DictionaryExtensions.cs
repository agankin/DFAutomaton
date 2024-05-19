using PureMonads;

namespace DFAutomaton;

internal static class DictionaryExtensions
{
    public static Option<TValue> GetOrNone<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key)
        where TKey : notnull
    {
        return dict.TryGetValue(key, out var value)
            ? value
            : Option.None<TValue>();
    }

    public static TValue GetOr<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key, Func<TValue> getDefaultValue)
        where TKey : notnull
    {
        return dict.TryGetValue(key, out var value)
            ? value
            : getDefaultValue();
    }
}