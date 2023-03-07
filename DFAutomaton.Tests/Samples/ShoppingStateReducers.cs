namespace DFAutomaton.Tests
{
    public class ShoppingStateReducers
    {
        public const decimal BreadPrice = 5;
        public const decimal ButterPrice = 10;

        public static readonly Func<ShoppingState, ShoppingState> AddBread =
            state => state with
            {
                GoodsCost = state.GoodsCost + BreadPrice
            };

        public static readonly Func<ShoppingState, ShoppingState> AddButter =
            state => state with
            {
                GoodsCost = state.GoodsCost + ButterPrice
            };

        public static readonly Func<ShoppingState, ShoppingState> PayForGoods =
            state => state with
            {
                Type = ShoppingStateType.GoodsPaid
            };

        public static readonly Func<ShoppingState, ShoppingState> ReceiveGoods =
            state => state with
            {
                Type = ShoppingStateType.GoodsPurchased
            };
    }
}