namespace DFAutomaton.Tests;

public class ShoppingStateReducers
{
    public const decimal BreadPrice = 5;
    public const decimal ButterPrice = 10;

    public static readonly Reducer<ShoppingActions, ShoppingState> AddBread =
        (_, state) => state with
        {
            GoodsCost = state.GoodsCost + BreadPrice
        };

    public static readonly Reducer<ShoppingActions, ShoppingState> AddButter =
        (_, state) => state with
        {
            GoodsCost = state.GoodsCost + ButterPrice
        };

    public static readonly Reducer<ShoppingActions, ShoppingState> PayForGoods =
        (_, state) => state with
        {
            Type = ShoppingStateType.GoodsPaid
        };

    public static readonly Reducer<ShoppingActions, ShoppingState> ReceiveGoods =
        (_, state) => state with
        {
            Type = ShoppingStateType.GoodsPurchased
        };
}