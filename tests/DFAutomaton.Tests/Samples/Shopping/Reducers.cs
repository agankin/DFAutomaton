using Optional;

namespace DFAutomaton.Tests.Samples.Shopping;

using StateOrError = Option<State, Errors>;

public static class Reducers
{
    public static readonly Reduce<Actions, StateOrError> PutBreadToCart =
        (stateOrError, _) => stateOrError.Map(state => state with { Cart = state.Cart.Put(Goods.Bread) });

    public static readonly Reduce<Actions, StateOrError> PutButterToCart =
        (stateOrError, _) => stateOrError.Map(state => state with { Cart = state.Cart.Put(Goods.Butter) });

    public static readonly Reduce<Actions, StateOrError> RemoveBreadFromCart =
        (stateOrError, _) => stateOrError.Map(state => state with { Cart = state.Cart.Remove(Goods.Bread) });

    public static readonly Reduce<Actions, StateOrError> RemoveButterFromCart =
        (stateOrError, _) => stateOrError.Map(state => state with { Cart = state.Cart.Remove(Goods.Butter) });

    public static readonly Reduce<Actions, StateOrError> Pay =
       (stateOrError, _) => stateOrError.FlatMap(state => state.Pay());

    public static readonly Reduce<Actions, StateOrError> Purchase =
        (stateOrError, _) => stateOrError.FlatMap(state => state.Purchase());
}