namespace DFAutomaton.Tests;

public static class AutomatonErrorExtensions
{
    public static AutomatonError<TTransition, TState> HasType<TTransition, TState>(this AutomatonError<TTransition, TState> error, AutomatonErrorType expectedType)
        where TTransition : notnull
    {
        error.Type.ItIs(expectedType);
        return error;
    }

    public static AutomatonError<TTransition, TState> OccuredOn<TTransition, TState>(this AutomatonError<TTransition, TState> error, TTransition expectedTransition)
        where TTransition : notnull
    {
        var transition = error.Transition.IsSome();
        transition.ItIs(expectedTransition);

        return error;
    }
}