using Optional.Unsafe;

namespace DFAutomaton.Tests;

public class ShoppingStateGraph
{
    public Automaton<ShoppingActions, ShoppingState> Automaton { get; init; } = null!;

    public State<ShoppingActions, ShoppingState> ShoppingState { get; init; } = null!;
    
    public State<ShoppingActions, ShoppingState> PaidState { get; init; } = null!;

    public static ShoppingStateGraph Create()
    {
        var builder = AutomatonBuilder<ShoppingActions, ShoppingState>.Create();
        var shoppingState = builder.Start;

        shoppingState.LinkFixedState(ShoppingActions.AddBread, shoppingState, ShoppingStateReducers.AddBread);
        shoppingState.LinkFixedState(ShoppingActions.AddButter, shoppingState, ShoppingStateReducers.AddButter);
        var paidState = shoppingState
            .ToNewFixedState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);
        paidState
            .ToAccepted(ShoppingActions.ReceiveGoods, ShoppingStateReducers.ReceiveGoods);

        return new ShoppingStateGraph
        {
            Automaton = builder.Build().ValueOrFailure(),
            ShoppingState = shoppingState,
            PaidState = paidState
        };
    }
}