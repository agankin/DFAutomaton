namespace DFAutomaton.Utils;

internal static class StateIdGenerator
{
    public static Func<long> CreateNew()
    {
        long currentId = 0;
        return () => currentId++;
    }
}