using Optional;

namespace DFAutomaton;

internal static class ReducerFactory
{
    public static Reduce<TTransition, TState> Create<TTransition, TState>(TState reducedStateValue) where TTransition : notnull
    {
        Reduce<TTransition, TState> reduce = _ =>
        {
            var noneDynamicGoTo = Option.None<IState<TTransition, TState>>();
            return new ReductionResult<TTransition, TState>(reducedStateValue, GoToState: noneDynamicGoTo);
        };

        return reduce;
    }

    public static Reduce<TTransition, TState> Create<TTransition, TState>(ReduceValue<TTransition, TState> reduceValue)
        where TTransition : notnull
    {
        Reduce<TTransition, TState> reduce = automatonTransition =>
        {
            var transition = automatonTransition.Transition;
            var prevStateValue = automatonTransition.StateValueBefore;
            
            var nextStateValue = reduceValue(transition, prevStateValue);
            var noneDynamicGoTo = Option.None<IState<TTransition, TState>>();

            return new ReductionResult<TTransition, TState>(nextStateValue, GoToState: noneDynamicGoTo);
        };

        return reduce;
    }

    public static Reduce<TTransition, TState> Create<TTransition, TState>(ReduceTransition<TTransition, TState> reduceTransition)
        where TTransition : notnull
    {
        Reduce<TTransition, TState> reduce = automatonTransition =>
        {
            var nextStateValue = reduceTransition(automatonTransition);
            var noneDynamicGoTo = Option.None<IState<TTransition, TState>>();
            
            return new ReductionResult<TTransition, TState>(nextStateValue, GoToState: noneDynamicGoTo);
        };

        return reduce;
    }
}