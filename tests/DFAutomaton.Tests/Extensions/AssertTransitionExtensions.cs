using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests;

public static class AssertTransitionExtensions
{
    public static void AssertTransition<TTransition, TState>(
        this Option<IState<TTransition, TState>.Transition> stateTransitionOption,
        IState<TTransition, TState> expectedNextState,
        ReduceValue<TTransition, TState> expectedReduce)
        where TTransition : notnull
    {
        stateTransitionOption.AssertSome(stateTransition =>
        {
            var (_, nextStateOption, reduce) = stateTransition;

            nextStateOption.AssertSome(nextState => Assert.AreEqual(expectedNextState, nextState));
            Assert.AreEqual(expectedReduce, reduce);
        });
    }

    public static void AssertTransition<TTransition, TState>(
        this Option<State<TTransition, TState>.Transition> stateTransitionOption,
        State<TTransition, TState> expectedNextState,
        ReduceValue<TTransition, TState> expectedReduce)
        where TTransition : notnull
    {
        stateTransitionOption.AssertSome(stateTransition =>
        {
            var (_, nextStateOption, reduce) = stateTransition;

            nextStateOption.AssertSome(nextState => Assert.AreEqual(expectedNextState, nextState));
            Assert.AreEqual(expectedReduce, reduce);
        });
    }

    public static void AssertTransition<TTransition, TState>(
        this Option<State<TTransition, TState>.Transition> stateTransitionOption,
        State<TTransition, TState> expectedNextState,
        TState valueToReduce,
        TState expectedReducedValue)
        where TTransition : notnull
    {
        stateTransitionOption.AssertSome(stateTransition =>
        {
            var (_, nextStateOption, reduce) = stateTransition;

            nextStateOption.AssertSome(nextState => Assert.AreEqual(expectedNextState, nextState));

            var automatonState = new AutomatonState<TTransition, TState>(
                valueToReduce,
                nextStateOption.Map<IState<TTransition, TState>>(_ => _),
                _ => {});
            Assert.AreEqual(expectedReducedValue, reduce(automatonState));
        });
    }

    public static void AssertTransitionToAccepted<TTransition, TState>(
        this Option<State<TTransition, TState>.Transition> stateTransitionOption,
        ReduceValue<TTransition, TState> expectedReduce)
        where TTransition : notnull
    {
        stateTransitionOption.AssertSome(stateTransition =>
        {
            var (_, nextStateOption, reduce) = stateTransition;

            nextStateOption.AssertSome(nextState => Assert.AreEqual(StateType.Accepted, nextState.Type));
            Assert.AreEqual(expectedReduce, reduce);
        });
    }

    public static void AssertTransitionToAccepted<TTransition, TState>(
        this Option<State<TTransition, TState>.Transition> stateTransitionOption,
        TState valueToReduce,
        TState expectedReducedValue)
        where TTransition : notnull
    {
        stateTransitionOption.AssertSome(stateTransition =>
        {
            var (_, nextStateOption, reduce) = stateTransition;

            nextStateOption.AssertSome(nextState => Assert.AreEqual(StateType.Accepted, nextState.Type));
            
            var automatonState = new AutomatonState<TTransition, TState>(
                valueToReduce,
                nextStateOption.Map<IState<TTransition, TState>>(_ => _),
                _ => {});
            Assert.AreEqual(expectedReducedValue, reduce(automatonState));
        });
    }
}