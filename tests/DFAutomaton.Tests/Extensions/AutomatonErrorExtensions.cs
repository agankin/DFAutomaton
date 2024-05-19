using NUnit.Framework;

namespace DFAutomaton.Tests;

public static class AutomatonErrorExtensions
{
    public static AutomatonError<TTransition, TState> HasType<TTransition, TState>(this AutomatonError<TTransition, TState> error, AutomatonErrorType expectedType)
        where TTransition : notnull
    {
        Assert.AreEqual(expectedType, error.Type);
        return error;
    }

    public static AutomatonError<TTransition, TState> OccuredOn<TTransition, TState>(this AutomatonError<TTransition, TState> error, TTransition expectedTransition)
        where TTransition : notnull
    {
        Assert.AreEqual(expectedTransition, error.Transition);
        return error;
    }
}