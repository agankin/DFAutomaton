using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests
{
    public static class AssertAutomataStateExtensions
    {
        public static void AssertSomeNextState<TTransition, TState>(
            this Option<Next<TTransition, TState>> nextStateReducerOption,
            IState<TTransition, TState> expectedNextState,
            StateReducer<TTransition, TState> expectedReducer)
            where TTransition : notnull
        {
            nextStateReducerOption.AssertSome(nextStateReducer =>
            {
                var (nextState, reducer) = nextStateReducer;

                Assert.AreEqual(expectedNextState, nextState);
                Assert.AreEqual(expectedReducer, reducer);
            });
        }
    }
}