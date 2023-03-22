namespace DFAutomaton.Tests
{
    public class ShoppingStateGraph
    {
        public State<ShoppingActions, ShoppingState> ShoppingState { get; init; } = null!;
        
        public State<ShoppingActions, ShoppingState> PaidState { get; init; } = null!;

        public static ShoppingStateGraph Create()
        {
            var shoppingState = StateFactory<ShoppingActions, ShoppingState>.Start();

            shoppingState.LinkState(ShoppingActions.AddBread, shoppingState, ShoppingStateReducers.AddBread);
            shoppingState.LinkState(ShoppingActions.AddButter, shoppingState, ShoppingStateReducers.AddButter);
            var paidState = shoppingState
                .ToNewState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);
            paidState
                .ToNewAccepted(ShoppingActions.ReceiveGoods, ShoppingStateReducers.ReceiveGoods);

            return new ShoppingStateGraph
            {
                ShoppingState = shoppingState,
                PaidState = paidState
            };
        }
    }
}