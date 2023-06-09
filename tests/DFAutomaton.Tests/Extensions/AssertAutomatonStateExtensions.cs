using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests;

public static class AssertAutomatonStateExtensions
{
    public static void AssertTransition<TTransition, TState>(
        this Option<StateTransition<TTransition, TState>> stateTransitionOption,
        IState<TTransition, TState> expectedNextState,
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
}