namespace DFAutomaton.Tests;

public class ShoppingStateReducers
{
    public const decimal BreadPrice = 5;
    public const decimal ButterPrice = 10;

    public static readonly StateReducer<ShoppingActions, ShoppingState> AddBread =
        (_, state) => state with
        {
            GoodsCost = state.GoodsCost + BreadPrice
        };

    public static readonly StateReducer<ShoppingActions, ShoppingState> AddButter =
        (_, state) => state with
        {
            GoodsCost = state.GoodsCost + ButterPrice
        };

    public static readonly StateReducer<ShoppingActions, ShoppingState> PayForGoods =
        (_, state) => state with
        {
            Type = ShoppingStateType.GoodsPaid
        };

    public static readonly StateReducer<ShoppingActions, ShoppingState> ReceiveGoods =
        (_, state) => state with
        {
            Type = ShoppingStateType.GoodsPurchased
        };
}