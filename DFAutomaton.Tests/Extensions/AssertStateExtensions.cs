using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests
{
    public static class AssertStateExtensions
    {
        public static void AssertSomeNextState<TTransition, TState>(
            this Option<NextState<TTransition, TState>> nextStateReducerOption,
            State<TTransition, TState> expectedNextState,
            Func<TState, TState> expectedReducer)
            where TTransition : notnull
        {
            nextStateReducerOption.AssertSome(nextStateReducer =>
            {
                var (nextState, reducer) = nextStateReducer;

                Assert.AreEqual(expectedNextState, nextState);
                Assert.AreEqual(expectedReducer, reducer);
            });
        }

        public static void AssertSomeNextState<TTransition, TState>(
            this Option<NextState<TTransition, TState>> nextStateReducerOption,
            State<TTransition, TState> expectedNextState,
            TState valueToReduce,
            TState expectedReducedValue)
            where TTransition : notnull
        {
            nextStateReducerOption.AssertSome(nextStateReducer =>
            {
                var (nextState, reducer) = nextStateReducer;

                Assert.AreEqual(expectedNextState, nextState);
                Assert.AreEqual(expectedReducedValue, reducer(valueToReduce));
            });
        }

        public static void AssertSomeAccepted<TTransition, TState>(
            this Option<NextState<TTransition, TState>> nextStateReducerOption,
            Func<TState, TState> expectedReducer)
            where TTransition : notnull
        {
            nextStateReducerOption.AssertSome(nextStateReducer =>
            {
                var (nextState, reducer) = nextStateReducer;

                Assert.AreEqual(StateType.Accepted, nextState.Type);
                Assert.AreEqual(expectedReducer, reducer);
            });
        }

        public static void AssertSomeAccepted<TTransition, TState>(
            this Option<NextState<TTransition, TState>> nextStateReducerOption,
            TState valueToReduce,
            TState expectedReducedValue)
            where TTransition : notnull
        {
            nextStateReducerOption.AssertSome(nextStateReducer =>
            {
                var (nextState, reducer) = nextStateReducer;

                Assert.AreEqual(StateType.Accepted, nextState.Type);
                Assert.AreEqual(expectedReducedValue, reducer(valueToReduce));
            });
        }
    }
}