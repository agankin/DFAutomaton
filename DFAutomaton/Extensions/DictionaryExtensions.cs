using Optional;

namespace DFAutomaton.Extensions
{
    public static class DictionaryExtensions
    {
        public static Option<TValue> Get<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TKey : notnull
        {
            return dict.TryGetValue(key, out var value)
                ? Option.Some(value)
                : Option.None<TValue>();
        }
    }
}