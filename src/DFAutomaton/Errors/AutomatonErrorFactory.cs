using Optional;

namespace DFAutomaton;

internal static class AutomatonErrorFactory
{
    internal static AutomatonError<TTransition, TState> TransitionNotFound<TTransition, TState>(
        State<TTransition, TState> fromState,
        TTransition transition
    )
    where TTransition : notnull
    {
        var error = new AutomatonError<TTransition, TState>(
            AutomatonErrorType.TransitionNotExists,
            new ImmutableState<TTransition, TState>(fromState).Some(),
            transition.Some(),
            Option.None<TState>());
        
        return error;
    }

    internal static AutomatonError<TTransition, TState> TransitionFromAccepted<TTransition, TState>(
        State<TTransition, TState> fromState,
        TTransition transition
    ) where TTransition : notnull
    {
        var errorType = fromState.Type == StateType.Accepted
            ? AutomatonErrorType.TransitionFromAccepted
            : AutomatonErrorType.TransitionNotExists;
        var error = new AutomatonError<TTransition, TState>(
            errorType,
            new ImmutableState<TTransition, TState>(fromState).Some(),
            transition.Some(),
            Option.None<TState>());
        
        return error;
    }

    internal static AutomatonError<TTransition, TState> NoNextState<TTransition, TState>(
        State<TTransition, TState> fromState,
        TTransition transition
    )
    where TTransition : notnull
    {
        var error = new AutomatonError<TTransition, TState>(
            AutomatonErrorType.NoNextState,
            new ImmutableState<TTransition, TState>(fromState).Some(),
            transition.Some(),
            Option.None<TState>());
        
        return error;
    }

    internal static AutomatonError<TTransition, TState> AcceptedNotReached<TTransition, TState>() where TTransition : notnull
    {
        var error = new AutomatonError<TTransition, TState>(
            AutomatonErrorType.AcceptedNotReached,
            Option.None<ImmutableState<TTransition, TState>>(),
            Option.None<TTransition>(),
            Option.None<TState>()
        );
        
        return error;
    }

    internal static AutomatonError<TTransition, TState> ReducerError<TTransition, TState>(
        State<TTransition, TState> fromState,
        TTransition transition,
        TState errorState
    ) where TTransition : notnull
    {
        var error = new AutomatonError<TTransition, TState>(
            AutomatonErrorType.ReducerError,
            new ImmutableState<TTransition, TState>(fromState).Some(),
            transition.Some(),
            errorState.Some());
        
        return error;
    }
}