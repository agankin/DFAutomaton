using NUnit.Framework;

namespace DFAutomaton.Tests;

public static class AssertStateExtensions
{
    public static State<TTransition, TState> Has<TTransition, TState>(
        this State<TTransition, TState> state,
        StateType expectedType)
        where TTransition : notnull
    {
        Assert.That(state.Type, Is.EqualTo(expectedType));
        return state;
    }

    public static FrozenState<TTransition, TState> Has<TTransition, TState>(
        this FrozenState<TTransition, TState> state,
        StateType expectedType)
        where TTransition : notnull
    {
        Assert.That(state.Type, Is.EqualTo(expectedType));
        return state;
    }
}