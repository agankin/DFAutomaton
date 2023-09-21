using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests;

public static class AssertTransitionExtensions
{
    public static IState<TTransition, TState>.Transition TransitsTo<TTransition, TState>(
        this IState<TTransition, TState>.Transition transition,
        TransitionKind expectedKind)
        where TTransition : notnull
    {
        Assert.AreEqual(expectedKind, transition.Kind);
        return transition;
    }

    public static State<TTransition, TState>.Transition TransitsTo<TTransition, TState>(
        this State<TTransition, TState>.Transition transition,
        TransitionKind expectedKind)
        where TTransition : notnull
    {
        Assert.AreEqual(expectedKind, transition.Kind);
        return transition;
    }

    public static State<TTransition, TState>.Transition TransitsTo<TTransition, TState>(
        this State<TTransition, TState>.Transition transition,
        State<TTransition, TState> expectedState)
        where TTransition : notnull
    {
        Assert.AreEqual(expectedState, transition.State);
        return transition;
    }

    public static State<TTransition, TState>.Transition TransitsTo<TTransition, TState>(
        this State<TTransition, TState>.Transition transition,
        AcceptedState<TTransition, TState> expectedState)
        where TTransition : notnull
    {
        return transition.TransitsTo(expectedState.State);
    }

    public static IState<TTransition, TState>.Transition Reduces<TTransition, TState>(
        this IState<TTransition, TState>.Transition transition,
        TState beforeReduce,
        TState expectedAfterReduce)
        where TTransition : notnull
    {
        var automatonState = new AutomatonState<TTransition, TState>(
            beforeReduce,
            Option.None<IState<TTransition, TState>>(),
            _ => {});
        var actualAfterReduce = transition.Reduce(automatonState).State;
        
        Assert.AreEqual(expectedAfterReduce, actualAfterReduce);
        
        return transition;
    }

    public static State<TTransition, TState>.Transition Reduces<TTransition, TState>(
        this State<TTransition, TState>.Transition transition,
        TState beforeReduce,
        TState expectedAfterReduce)
        where TTransition : notnull
    {
        var automatonState = new AutomatonState<TTransition, TState>(
            beforeReduce,
            Option.None<IState<TTransition, TState>>(),
            _ => {});
        var actualAfterReduce = transition.Reduce(automatonState).State;
        
        Assert.AreEqual(expectedAfterReduce, actualAfterReduce);
        
        return transition;
    }
}