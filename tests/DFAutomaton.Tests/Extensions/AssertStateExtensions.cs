using NUnit.Framework;

namespace DFAutomaton.Tests;

public static class AssertStateExtensions
{
    public static State<TTransition, TState> Is<TTransition, TState>(
        this State<TTransition, TState> state,
        State<TTransition, TState> expectedState)
        where TTransition : notnull
    {
        Assert.AreEqual(expectedState, state);
        return state;
    }

    public static State<TTransition, TState> Has<TTransition, TState>(
        this State<TTransition, TState> state,
        StateType expectedType)
        where TTransition : notnull
    {
        Assert.AreEqual(expectedType, state.Type);
        return state;
    }

    public static FrozenState<TTransition, TState> Has<TTransition, TState>(
        this FrozenState<TTransition, TState> state,
        StateType expectedType)
        where TTransition : notnull
    {
        Assert.AreEqual(expectedType, state.Type);
        return state;
    }
}