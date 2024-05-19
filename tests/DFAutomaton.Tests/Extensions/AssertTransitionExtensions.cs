using NUnit.Framework;

namespace DFAutomaton.Tests;

public static class AssertTransitionExtensions
{
    public static Transition<TTransition, TState> TransitsTo<TTransition, TState>(
        this Transition<TTransition, TState> transition,
        State<TTransition, TState> expectedState)
        where TTransition : notnull
    {
        var transitionState = transition.ToState.IsSome();
        Assert.AreEqual(expectedState, transitionState);
        
        return transition;
    }

    public static Transition<TTransition, TState> TransitsTo<TTransition, TState>(
        this Transition<TTransition, TState> transition,
        StateType expectedStateType)
        where TTransition : notnull
    {
        var transitionState = transition.ToState.IsSome();
        Assert.AreEqual(expectedStateType, transitionState.Type);
        
        return transition;
    }

    public static Transition<TTransition, TState> TransitsDynamicly<TTransition, TState>(this Transition<TTransition, TState> transition)
        where TTransition : notnull
    {
        transition.ToState.IsNone();
        return transition;
    }

    public static Transition<TTransition, TState> Reduces<TTransition, TState>(
        this Transition<TTransition, TState> stateTransition,
        TTransition transition,
        TState beforeReduce,
        TState expectedAfterReduce)
        where TTransition : notnull
    {
        var actualAfterReduce = stateTransition.Reducer(beforeReduce, transition);   
        Assert.AreEqual(expectedAfterReduce, actualAfterReduce);
        
        return stateTransition;
    }
}