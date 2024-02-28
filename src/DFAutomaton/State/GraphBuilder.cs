﻿using Optional;

namespace DFAutomaton;

internal static class GraphBuilder
{
    public static Option<State<TTransition, TState>, ValidationError> Build<TTransition, TState>(this State<TTransition, TState> startState, ValidationConfiguration configuration)
        where TTransition : notnull
    {
        var start = startState.AsImmutable();

        return configuration.ValidateAnyReachesAccepted
            ? StateGraphValidator<TTransition, TState>.ValidateHasAccepted(start)
                .FlatMap(_ => StateGraphValidator<TTransition, TState>.ValidateAnyReachAccepted(start))
            : start.Some<State<TTransition, TState>, ValidationError>();
    }

    private static State<TTransition, TState> AsImmutable<TTransition, TState>(this State<TTransition, TState> current)
        where TTransition : notnull
    {
        return current;
    }
}