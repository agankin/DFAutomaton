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

    public static IState<TTransition, TState>.Transition TransitsTo<TTransition, TState>(
        this IState<TTransition, TState>.Transition transition,
        IState<TTransition, TState> expectedState)
        where TTransition : notnull
    {
        var transitionState = transition.State.IsSome();
        Assert.AreEqual(expectedState, transitionState);
        
        return transition;
    }

    public static State<TTransition, TState>.Transition TransitsTo<TTransition, TState>(
        this State<TTransition, TState>.Transition transition,
        State<TTransition, TState> expectedState)
        where TTransition : notnull
    {
        var transitionState = transition.State.IsSome();
        Assert.AreEqual(expectedState, transitionState);
        
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
        this IState<TTransition, TState>.Transition stateTransition,
        TTransition transition,
        TState beforeReduce,
        TState expectedAfterReduce)
        where TTransition : notnull
    {
        var automatonTransition = new AutomatonTransition<TTransition, TState>(
            beforeReduce,
            transition,
            Option.None<IState<TTransition, TState>>(),
            _ => {});
        var actualAfterReduce = stateTransition.Reduce(automatonTransition).StateValue;
        
        Assert.AreEqual(expectedAfterReduce, actualAfterReduce);
        
        return stateTransition;
    }

    public static State<TTransition, TState>.Transition Reduces<TTransition, TState>(
        this State<TTransition, TState>.Transition stateTransition,
        TTransition transition,
        TState beforeReduce,
        TState expectedAfterReduce)
        where TTransition : notnull
    {
        var automatonTransition = new AutomatonTransition<TTransition, TState>(
            beforeReduce,
            transition,
            Option.None<IState<TTransition, TState>>(),
            _ => {});
        var actualAfterReduce = stateTransition.Reduce(automatonTransition).StateValue;
        
        Assert.AreEqual(expectedAfterReduce, actualAfterReduce);
        
        return stateTransition;
    }
}