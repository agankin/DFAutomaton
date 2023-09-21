using Optional;

namespace DFAutomaton.Tests.Samples.Shopping;

using StateOrError = Option<State, Errors>;

public static class Reducers
{
    public static readonly ReduceValue<Actions, StateOrError> PutBreadToCart =
        automatonState => automatonState.Reduce(state => state with { Cart = state.Cart.Put(Goods.Bread) });

    public static readonly ReduceValue<Actions, StateOrError> PutButterToCart =
        automatonState => automatonState.Reduce(state => state with { Cart = state.Cart.Put(Goods.Butter) });

    public static readonly ReduceValue<Actions, StateOrError> RemoveBreadFromCart =
        automatonState => automatonState.Reduce(state => state with { Cart = state.Cart.Remove(Goods.Bread) });

    public static readonly ReduceValue<Actions, StateOrError> RemoveButterFromCart =
        automatonState => automatonState.Reduce(state => state with { Cart = state.Cart.Remove(Goods.Butter) });

    public static readonly ReduceValue<Actions, StateOrError> Pay =
        automatonState => automatonState.Reduce(state => state.Pay());

    public static readonly ReduceValue<Actions, StateOrError> Purchase =
        automatonState => automatonState.Reduce(state => state.Purchase());

    private static StateOrError Reduce(this AutomatonState<Actions, StateOrError> automatonState, Func<State, State> reduce)
    {
        var stateOrError = automatonState.CurrentValue;
        return stateOrError.Map(reduce);
    }

    private static StateOrError Reduce(this AutomatonState<Actions, StateOrError> automatonState, Func<State, StateOrError> reduce)
    {
        var stateOrError = automatonState.CurrentValue;
        return stateOrError.FlatMap(reduce);
    }
}