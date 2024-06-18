using DFAutomaton.Tests;

namespace DFAutomaton;

internal static class Reducer
{
    public static Reduce<TTransition, TState> Create<TTransition, TState>(TTransition expectedTransition, TState reducedState)
        where TTransition : notnull
    {
        return (state, transition) =>
        {
            transition.ItIs(expectedTransition);
            return reducedState;
        };
    }
} 