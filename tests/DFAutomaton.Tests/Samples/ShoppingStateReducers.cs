namespace DFAutomaton.Tests;

public class ShoppingStateReducers
{
    public const decimal BreadPrice = 5;
    public const decimal ButterPrice = 10;

    public static readonly ReduceValue<ShoppingActions, ShoppingState> AddBread =
        state => state.CurrentValue with
        {
            GoodsCost = state.CurrentValue.GoodsCost + BreadPrice
        };

    public static readonly ReduceValue<ShoppingActions, ShoppingState> AddButter =
        state => state.CurrentValue with
        {
            GoodsCost = state.CurrentValue.GoodsCost + ButterPrice
        };

    public static readonly ReduceValue<ShoppingActions, ShoppingState> PayForGoods =
        state => state.CurrentValue with
        {
            Type = ShoppingStateType.GoodsPaid
        };

    public static readonly ReduceValue<ShoppingActions, ShoppingState> ReceiveGoods =
        state => state.CurrentValue with
        {
            Type = ShoppingStateType.GoodsPurchased
        };
}