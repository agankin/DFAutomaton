using PureMonads;

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
            new ImmutableState<TTransition, TState>(fromState),
            transition,
            Option.None<TState>());
        
        return error;
    }

    internal static AutomatonError<TTransition, TState> TransitionFromAccepted<TTransition, TState>(
        State<TTransition, TState> fromState,
        TTransition transition
    ) where TTransition : notnull
    {
        var error = new AutomatonError<TTransition, TState>(
            AutomatonErrorType.TransitionFromAccepted,
            new ImmutableState<TTransition, TState>(fromState),
            transition,
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
            new ImmutableState<TTransition, TState>(fromState),
            transition,
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
            new ImmutableState<TTransition, TState>(fromState),
            transition,
            errorState);
        
        return error;
    }
}