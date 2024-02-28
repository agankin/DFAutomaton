using NUnit.Framework;
using Optional;

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