using PureMonads;

namespace DFAutomaton;

using static AutomatonErrorType;
using static Option;

internal static class AutomatonErrorFactory
{
    internal static AutomatonError<TTransition, TState> CreateTransitionNotExists<TTransition, TState>(
        FrozenState<TTransition, TState> fromState,
        TTransition transition)
        where TTransition : notnull
    {
        return new(TransitionNotExists, fromState, transition, None<TState>());
    }

    internal static AutomatonError<TTransition, TState> CreateTransitionFromAccepted<TTransition, TState>(
        FrozenState<TTransition, TState> fromState,
        TTransition transition)
        where TTransition : notnull
    {
        return new(TransitionFromAccepted, fromState, transition, None<TState>());
    }

    internal static AutomatonError<TTransition, TState> CreateNoNextState<TTransition, TState>(
        FrozenState<TTransition, TState> fromState,
        TTransition transition)
        where TTransition : notnull
    {
        return new(NoNextState, fromState, transition, None<TState>());
    }

    internal static AutomatonError<TTransition, TState> CreateAcceptedNotReached<TTransition, TState>()
        where TTransition : notnull
    {
       return new(AcceptedNotReached, None<FrozenState<TTransition, TState>>(), None<TTransition>(), None<TState>());
    }

    internal static AutomatonError<TTransition, TState> CreateReducerError<TTransition, TState>(
        FrozenState<TTransition, TState> fromState,
        TTransition transition,
        TState errorState)
        where TTransition : notnull
    {
        return new(ReducerError, fromState, transition, errorState);
    }
}