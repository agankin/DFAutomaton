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
}