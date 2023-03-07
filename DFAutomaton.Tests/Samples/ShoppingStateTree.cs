namespace DFAutomaton.Tests
{
    public class ShoppingStateTree
    {
        public State<ShoppingActions, ShoppingState> ShoppingState { get; init; } = null!;
        
        public State<ShoppingActions, ShoppingState> PaidState { get; init; } = null!;

        public static ShoppingStateTree Create()
        {
            var shoppingState = StateFactory<ShoppingActions, ShoppingState>.Start();

            shoppingState.LinkState(ShoppingActions.AddBread, shoppingState, ShoppingStateReducers.AddBread);
            shoppingState.LinkState(ShoppingActions.AddButter, shoppingState, ShoppingStateReducers.AddButter);
            var paidState = shoppingState
                .ToNewState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);
            paidState
                .ToNewAccepted(ShoppingActions.ReceiveGoods, ShoppingStateReducers.ReceiveGoods);

            return new ShoppingStateTree
            {
                ShoppingState = shoppingState,
                PaidState = paidState
            };
        }
    }
}