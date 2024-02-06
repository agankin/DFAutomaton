namespace DFAutomaton.Utils;

/// <summary>
/// The factory to create generators of sequential ids.
/// </summary>
internal static class SeqIdGenerator
{
    /// <summary>
    /// Creates a new generator that produces sequentially incremened ids.
    /// </summary>
    public static Func<long> CreateNew()
    {
        long currentId = 0;
        return () => currentId++;
    }
}