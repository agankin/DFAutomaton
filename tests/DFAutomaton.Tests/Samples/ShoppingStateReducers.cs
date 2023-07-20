namespace DFAutomaton.Tests;

public class ShoppingStateReducers
{
    public const decimal BreadPrice = 5;
    public const decimal ButterPrice = 10;

    public static readonly Reduce<ShoppingState> AddBread =
        state => state with
        {
            GoodsCost = state.GoodsCost + BreadPrice
        };

    public static readonly Reduce<ShoppingState> AddButter =
        state => state with
        {
            GoodsCost = state.GoodsCost + ButterPrice
        };

    public static readonly Reduce<ShoppingState> PayForGoods =
        state => state with
        {
            Type = ShoppingStateType.GoodsPaid
        };

    public static readonly Reduce<ShoppingState> ReceiveGoods =
        state => state with
        {
            Type = ShoppingStateType.GoodsPurchased
        };
}