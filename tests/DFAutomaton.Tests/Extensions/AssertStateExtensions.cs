using NUnit.Framework;

namespace DFAutomaton.Tests;

public static class AssertStateExtensions
{
    public static State<TTransition, TState> Is<TTransition, TState>(this State<TTransition, TState> state, StateType expectedType)
        where TTransition : notnull
    {
        Assert.AreEqual(expectedType, state.Type);
        return state;
    }

    public static IState<TTransition, TState> Is<TTransition, TState>(this IState<TTransition, TState> state, StateType expectedType)
        where TTransition : notnull
    {
        Assert.AreEqual(expectedType, state.Type);
        return state;
    }

    public static State<TTransition, TState> Is<TTransition, TState>(this AcceptedState<TTransition, TState> state, StateType expectedType)
        where TTransition : notnull
    {
        return state.State.Is(expectedType);
    }
}