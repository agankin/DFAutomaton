using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests;

public static class AssertMoveExtensions
{
    public static void AssertMove<TTransition, TState>(
        this Option<IState<TTransition, TState>.Move> stateMoveOption,
        IState<TTransition, TState> expectedNextState,
        Reducer<TTransition, TState> expectedReducer)
        where TTransition : notnull
    {
        stateMoveOption.AssertSome(stateMove =>
        {
            var (nextState, reducer) = stateMove;

            Assert.AreEqual(expectedNextState, nextState);
            Assert.AreEqual(expectedReducer, reducer);
        });
    }

    public static void AssertMove<TTransition, TState>(
        this Option<State<TTransition, TState>.Move> stateMoveOption,
        State<TTransition, TState> expectedNextState,
        Reducer<TTransition, TState> expectedReducer)
        where TTransition : notnull
    {
        stateMoveOption.AssertSome(stateMove =>
        {
            var (nextState, reducer) = stateMove;

            Assert.AreEqual(expectedNextState, nextState);
            Assert.AreEqual(expectedReducer, reducer);
        });
    }

    public static void AssertMove<TTransition, TState>(
        this Option<State<TTransition, TState>.Move> stateMoveOption,
        State<TTransition, TState> expectedNextState,
        TState valueToReduce,
        TState expectedReducedValue)
        where TTransition : notnull
    {
        stateMoveOption.AssertSome(stateMove =>
        {
            var (nextState, reducer) = stateMove;
            var runState = new AutomatonRunState<TTransition, TState>(nextState, _ => { });

            Assert.AreEqual(expectedNextState, nextState);
            Assert.AreEqual(expectedReducedValue, reducer(runState, valueToReduce));
        });
    }

    public static void AssertMoveToAccepted<TTransition, TState>(
        this Option<State<TTransition, TState>.Move> stateMoveOption,
        Reducer<TTransition, TState> expectedReducer)
        where TTransition : notnull
    {
        stateMoveOption.AssertSome(stateMove =>
        {
            var (nextState, reducer) = stateMove;

            Assert.AreEqual(StateType.Accepted, nextState.Type);
            Assert.AreEqual(expectedReducer, reducer);
        });
    }

    public static void AssertMoveToAccepted<TTransition, TState>(
        this Option<State<TTransition, TState>.Move> stateMoveOption,
        TState valueToReduce,
        TState expectedReducedValue)
        where TTransition : notnull
    {
        stateMoveOption.AssertSome(stateMove =>
        {
            var (nextState, reducer) = stateMove;
            var runState = new AutomatonRunState<TTransition, TState>(nextState, _ => { });

            Assert.AreEqual(StateType.Accepted, nextState.Type);
            Assert.AreEqual(expectedReducedValue, reducer(runState, valueToReduce));
        });
    }
}