namespace DFAutomaton.Utils;

internal static class SequentialIdGenerator
{
    public static Func<long> CreateNew()
    {
        long currentId = 0;
        return () => currentId++;
    }
}