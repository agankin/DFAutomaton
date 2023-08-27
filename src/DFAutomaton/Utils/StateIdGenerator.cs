namespace DFAutomaton.Utils;

/// <summary>
/// State id generator factory.
/// </summary>
internal static class StateIdGenerator
{
    /// <summary>
    /// Creates new state id generator that produces sequential state ids.
    /// </summary>
    public static Func<long> CreateNew()
    {
        long currentId = 0;
        return () => currentId++;
    }
}