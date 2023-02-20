using NUnit.Framework;

namespace DFAutomaton.Tests
{
    public static class StateExtensions
    {
        public static void AssertReducedTo<TTransition, TState>(
            this NextState<TTransition, TState> nextState,
            TState stateValueToReduce,
            TState expectedReducedStateValue)
            where TTransition : notnull
        {
            var (_, reducer) = nextState;
            var reducedValue = reducer(stateValueToReduce);

            Assert.AreEqual(expectedReducedStateValue, reducedValue);
        }
    }
}