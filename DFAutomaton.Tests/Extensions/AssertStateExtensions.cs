using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests
{
    public static class AssertStateExtensions
    {
        public static void AssertTransition<TTransition, TState>(
            this Option<Transition<TTransition, TState>> stateTransitionOption,
            State<TTransition, TState> expectedNextState,
            StateReducer<TTransition, TState> expectedReducer)
            where TTransition : notnull
        {
            stateTransitionOption.AssertSome(transition =>
            {
                var (nextState, reducer) = transition;

                Assert.AreEqual(expectedNextState, nextState);
                Assert.AreEqual(expectedReducer, reducer);
            });
        }

        public static void AssertTransition<TTransition, TState>(
            this Option<Transition<TTransition, TState>> stateTransitionOption,
            State<TTransition, TState> expectedNextState,
            TState valueToReduce,
            TState expectedReducedValue)
            where TTransition : notnull
        {
            stateTransitionOption.AssertSome(transition =>
            {
                var (nextState, reducer) = transition;
                var runState = new AutomatonRunState<TTransition, TState>(nextState, _ => { });

                Assert.AreEqual(expectedNextState, nextState);
                Assert.AreEqual(expectedReducedValue, reducer(runState, valueToReduce));
            });
        }

        public static void AssertAccepted<TTransition, TState>(
            this Option<Transition<TTransition, TState>> stateTransitionOption,
            StateReducer<TTransition, TState> expectedReducer)
            where TTransition : notnull
        {
            stateTransitionOption.AssertSome(transition =>
            {
                var (nextState, reducer) = transition;

                Assert.AreEqual(StateType.Accepted, nextState.Type);
                Assert.AreEqual(expectedReducer, reducer);
            });
        }

        public static void AssertAccepted<TTransition, TState>(
            this Option<Transition<TTransition, TState>> stateTransitionOption,
            TState valueToReduce,
            TState expectedReducedValue)
            where TTransition : notnull
        {
            stateTransitionOption.AssertSome(transition =>
            {
                var (nextState, reducer) = transition;
                var runState = new AutomatonRunState<TTransition, TState>(nextState, _ => { });

                Assert.AreEqual(StateType.Accepted, nextState.Type);
                Assert.AreEqual(expectedReducedValue, reducer(runState, valueToReduce));
            });
        }
    }
}