using NUnit.Framework;

namespace DFAutomaton.Tests
{
    public static class StateExtensions
    {
        public static void AssertReducedTo<TTransition, TState>(
            this StateReducer<TTransition, TState> stateReducer,
            TState expectedReducedValue)
            where TTransition : notnull
        {
            var (_, reducer) = stateReducer;
            var reducedValue = reducer(default!);

            Assert.AreEqual(reducedValue, expectedReducedValue);
        }
    }
}