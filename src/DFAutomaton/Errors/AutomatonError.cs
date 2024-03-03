using Optional;

namespace DFAutomaton;

/// <summary>
/// Contains information about an automaton runtime error.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="Type">The error type.</param>
/// <param name="WhenTransitioningFrom">
/// Some automaton state the error occured when transitioning from.
/// </param>
/// <param name="Transition">A transition caused the error.</param>
/// <param name="RuntimeException">
/// Some exception for <see cref="AutomatonErrorType.ReducerError"/> and <see cref="AutomatonErrorType.RunError"/> or None for other types.
/// </param>
public record AutomatonError<TTransition, TState>(
    AutomatonErrorType Type,
    Option<State<TTransition, TState>> WhenTransitioningFrom,
    Option<TTransition> Transition,
    Option<Exception> RuntimeException
)
where TTransition : notnull
{
    internal static AutomatonError<TTransition, TState> TransitionNotFound(State<TTransition, TState> fromState, TTransition transition)
    {
        var errorType = fromState.Type == StateType.Accepted
            ? AutomatonErrorType.TransitionFromAccepted
            : AutomatonErrorType.TransitionNotExists;
        var error = new AutomatonError<TTransition, TState>(
            errorType,
            fromState.Some(),
            transition.Some(),
            Option.None<Exception>());
        
        return error;
    }

    internal static AutomatonError<TTransition, TState> ReducerError(State<TTransition, TState> fromState, TTransition transition, Exception exception)
    {
        var error = new AutomatonError<TTransition, TState>(
            AutomatonErrorType.ReducerError,
            fromState.Some(),
            transition.Some(),
            exception.Some());
        
        return error;
    }

    internal static AutomatonError<TTransition, TState> RuntimeError(Exception exception)
    {
        var error = new AutomatonError<TTransition, TState>(
            AutomatonErrorType.RunError,
            Option.None<State<TTransition, TState>>(),
            Option.None<TTransition>(),
            exception.Some());
        
        return error;
    }
}