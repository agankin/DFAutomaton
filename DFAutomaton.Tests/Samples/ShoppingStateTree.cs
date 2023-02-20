namespace DFAutomaton.Tests
{
    public class ShoppingStateTree
    {
        public const decimal BreadPrice = 5;
        public const decimal ButterPrice = 10;

        private readonly State<ShoppingActions, ShoppingState> _goodsPaidState;

        public ShoppingStateTree(
            State<ShoppingActions, ShoppingState> shoppingState,
            State<ShoppingActions, ShoppingState> goodsPaidState)
        {
            ShoppingState = shoppingState;
            _goodsPaidState = goodsPaidState;
        }

        public State<ShoppingActions, ShoppingState> ShoppingState { get; init; }

        public static ShoppingStateTree Create()
        {
            var shoppingState = StateFactory<ShoppingActions, ShoppingState>.Start();

            shoppingState.ToState(ShoppingActions.AddBread, shoppingState, state => state with
            {
                GoodsCost = state.GoodsCost + BreadPrice
            });
            shoppingState.ToState(ShoppingActions.AddButter, shoppingState, state => state with
            {
                GoodsCost = state.GoodsCost + ButterPrice
            });

            var goodsPaidState = shoppingState.ToState(ShoppingActions.PayForGoods, state => state with
            {
                Type = ShoppingStateType.GoodsPaid
            });

            goodsPaidState.ToAccepted(ShoppingActions.ReceiveGoods, state => state with
            {
                Type = ShoppingStateType.GoodsPurchased
            });

            return new ShoppingStateTree(shoppingState, goodsPaidState);
        }

        public bool IsGoodsPaidState(State<ShoppingActions, ShoppingState> goodsPaidState) =>
            _goodsPaidState.Equals(goodsPaidState);
    }
}