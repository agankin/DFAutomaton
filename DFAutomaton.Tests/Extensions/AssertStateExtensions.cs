using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests
{
    public static class AssertStateExtensions
    {
        public static void AssertSomeNextState<TTransition, TState>(
            this Option<NextState<TTransition, TState>> nextStateReducerOption,
            State<TTransition, TState> expectedNextState,
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

        public static void AssertSomeNextState<TTransition, TState>(
            this Option<NextState<TTransition, TState>> nextStateReducerOption,
            State<TTransition, TState> expectedNextState,
            TState valueToReduce,
            TState expectedReducedValue)
            where TTransition : notnull
        {
            var control = new AutomataControl<TTransition>(_ => { });

            nextStateReducerOption.AssertSome(nextStateReducer =>
            {
                var (nextState, reducer) = nextStateReducer;

                Assert.AreEqual(expectedNextState, nextState);
                Assert.AreEqual(expectedReducedValue, reducer(control, valueToReduce));
            });
        }

        public static void AssertSomeAccepted<TTransition, TState>(
            this Option<NextState<TTransition, TState>> nextStateReducerOption,
            StateReducer<TTransition, TState> expectedReducer)
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
            var control = new AutomataControl<TTransition>(_ => { });

            nextStateReducerOption.AssertSome(nextStateReducer =>
            {
                var (nextState, reducer) = nextStateReducer;

                Assert.AreEqual(StateType.Accepted, nextState.Type);
                Assert.AreEqual(expectedReducedValue, reducer(control, valueToReduce));
            });
        }
    }
}