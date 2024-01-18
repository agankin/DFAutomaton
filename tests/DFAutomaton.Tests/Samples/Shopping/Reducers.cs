using Optional;

namespace DFAutomaton.Tests.Samples.Shopping;

using StateOrError = Option<State, Errors>;

public static class Reducers
{
    public static readonly ReduceValue<Actions, StateOrError> PutBreadToCart =
        automatonTransition => automatonTransition.Reduce(state => state with { Cart = state.Cart.Put(Goods.Bread) });

    public static readonly ReduceValue<Actions, StateOrError> PutButterToCart =
        automatonTransition => automatonTransition.Reduce(state => state with { Cart = state.Cart.Put(Goods.Butter) });

    public static readonly ReduceValue<Actions, StateOrError> RemoveBreadFromCart =
        automatonTransition => automatonTransition.Reduce(state => state with { Cart = state.Cart.Remove(Goods.Bread) });

    public static readonly ReduceValue<Actions, StateOrError> RemoveButterFromCart =
        automatonTransition => automatonTransition.Reduce(state => state with { Cart = state.Cart.Remove(Goods.Butter) });

    public static readonly ReduceValue<Actions, StateOrError> Pay =
        automatonTransition => automatonTransition.Reduce(state => state.Pay());

    public static readonly ReduceValue<Actions, StateOrError> Purchase =
        automatonTransition => automatonTransition.Reduce(state => state.Purchase());

    private static StateOrError Reduce(this AutomatonTransition<Actions, StateOrError> automatonTransition, Func<State, State> reduce)
    {
        var stateOrError = automatonTransition.StateValueBefore;
        return stateOrError.Map(reduce);
    }

    private static StateOrError Reduce(this AutomatonTransition<Actions, StateOrError> automatonTransition, Func<State, StateOrError> reduce)
    {
        var stateOrError = automatonTransition.StateValueBefore;
        return stateOrError.FlatMap(reduce);
    }
}