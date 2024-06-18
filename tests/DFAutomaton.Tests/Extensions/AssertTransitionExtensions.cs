namespace DFAutomaton.Tests;

public static class AssertTransitionExtensions
{
    public static Transition<TTransition, TState> TransitsTo<TTransition, TState>(
        this Transition<TTransition, TState> transition,
        State<TTransition, TState> expectedState)
        where TTransition : notnull
    {
        var transitionState = transition.ToState.IsSome();
        transitionState.ItIs(expectedState);
        
        return transition;
    }

    public static Transition<TTransition, TState> TransitsTo<TTransition, TState>(
        this Transition<TTransition, TState> transition,
        StateType expectedStateType)
        where TTransition : notnull
    {
        var transitionState = transition.ToState.IsSome();
        transitionState.Type.ItIs(expectedStateType);
        
        return transition;
    }

    public static Transition<TTransition, TState> TransitsDynamicly<TTransition, TState>(this Transition<TTransition, TState> transition)
        where TTransition : notnull
    {
        transition.ToState.IsNone();
        return transition;
    }

    public static Transition<TTransition, TState> Reduces<TTransition, TState>(
        this Transition<TTransition, TState> transition,
        TTransition transitionValue,
        TState beforeReduce,
        TState expectedAfterReduce)
        where TTransition : notnull
    {
        var actualAfterReduce = transition.Reducer(beforeReduce, transitionValue);   
        actualAfterReduce.Value.ItIs(expectedAfterReduce);
        
        return transition;
    }
}