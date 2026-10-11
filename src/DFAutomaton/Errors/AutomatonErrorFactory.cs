using PureMonads;

namespace DFAutomaton;

using ErrorType = AutomatonErrorType;
using static Option;

internal static class AutomatonErrorFactory
{
    internal static AutomatonError<TTransition, TState> TransitionNotExists<TTransition, TState>(
        FrozenState<TTransition, TState> fromState,
        TTransition transition,
        TState stateValue)
        where TTransition : notnull
    {
        return new AutomatonError<TTransition, TState>(
            ErrorType.TransitionNotExists,
            fromState,
            transition,
            stateValue);
    }

    internal static AutomatonError<TTransition, TState> TransitionFromAccepted<TTransition, TState>(
        FrozenState<TTransition, TState> fromState,
        TTransition transition,
        TState stateValue)
        where TTransition : notnull
    {
        return new AutomatonError<TTransition, TState>(
            ErrorType.TransitionFromAccepted,
            fromState,
            transition,
            stateValue);
    }

    internal static AutomatonError<TTransition, TState> NoNextState<TTransition, TState>(
        FrozenState<TTransition, TState> fromState,
        TTransition transition,
        TState stateValue)
        where TTransition : notnull
    {
        return new AutomatonError<TTransition, TState>(
            ErrorType.NoNextState,
            fromState,
            transition,
            stateValue);
    }

    internal static AutomatonError<TTransition, TState> AcceptedNotReached<TTransition, TState>(TState stateValue)
        where TTransition : notnull
    {
        return new AutomatonError<TTransition, TState>(
            ErrorType.AcceptedNotReached,
            None<FrozenState<TTransition, TState>>(),
            None<TTransition>(),
            stateValue);
    }

    internal static AutomatonError<TTransition, TState> StateError<TTransition, TState>(
        FrozenState<TTransition, TState> fromState,
        TTransition transition,
        TState stateValue)
        where TTransition : notnull
    {
        return new AutomatonError<TTransition, TState>(
            ErrorType.StateError,
            fromState,
            transition,
            stateValue);
    }
}