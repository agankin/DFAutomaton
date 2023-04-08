using Optional.Unsafe;

namespace DFAutomaton.Tests
{
    public class ShoppingStateGraph
    {
        public Automata<ShoppingActions, ShoppingState> Automaton { get; init; } = null!;

        public State<ShoppingActions, ShoppingState> ShoppingState { get; init; } = null!;
        
        public State<ShoppingActions, ShoppingState> PaidState { get; init; } = null!;

        public static ShoppingStateGraph Create()
        {
            var builder = AutomataBuilder<ShoppingActions, ShoppingState>.Create();
            var shoppingState = builder.StartState;

            shoppingState.LinkState(ShoppingActions.AddBread, shoppingState, ShoppingStateReducers.AddBread);
            shoppingState.LinkState(ShoppingActions.AddButter, shoppingState, ShoppingStateReducers.AddButter);
            var paidState = shoppingState
                .ToNewState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);
            paidState
                .ToNewAccepted(ShoppingActions.ReceiveGoods, ShoppingStateReducers.ReceiveGoods);

            return new ShoppingStateGraph
            {
                Automaton = builder.Build().ValueOrFailure(),
                ShoppingState = shoppingState,
                PaidState = paidState
            };
        }
    }
}