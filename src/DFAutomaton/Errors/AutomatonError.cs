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
/// <param name="ErrorState">
/// Some state returned from a reducer and determined to be a error state.
/// Set when type is <see cref="AutomatonErrorType.ReducerError"/>.
/// </param>
public record AutomatonError<TTransition, TState>(
    AutomatonErrorType Type,
    Option<State<TTransition, TState>> WhenTransitioningFrom,
    Option<TTransition> Transition,
    Option<TState> ErrorState
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
            Option.None<TState>());
        
        return error;
    }

    internal static AutomatonError<TTransition, TState> AcceptedNotReached()
    {
        var error = new AutomatonError<TTransition, TState>(
            AutomatonErrorType.AcceptedNotReached,
            Option.None<State<TTransition, TState>>(),
            Option.None<TTransition>(),
            Option.None<TState>()
        );
        
        return error;
    }

    internal static AutomatonError<TTransition, TState> ReducerError(
        State<TTransition, TState> fromState,
        TTransition transition,
        TState errorState)
    {
        var error = new AutomatonError<TTransition, TState>(
            AutomatonErrorType.ReducerError,
            fromState.Some(),
            transition.Some(),
            errorState.Some());
        
        return error;
    }
}