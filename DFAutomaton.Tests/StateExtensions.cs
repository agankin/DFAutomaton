using NUnit.Framework;

namespace DFAutomaton.Tests
{
    public static class StateExtensions
    {
        public static void AssertValueEquals<TTransition, TState>(
            this State<TTransition, TState> state,
            TState expected)
            where TTransition : notnull
        {
            state.Value.Match(
                value => Assert.AreEqual(value, expected),
                () => Assert.Fail($"State value is None but expected to be {expected}"));
        }
    }
}