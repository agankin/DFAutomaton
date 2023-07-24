using NUnit.Framework;

namespace DFAutomaton.Tests;

[TestFixture]
public class AutomatonStateGraphValidationTests
{
    [Test]
    public void BuildWithoutAccepted()
    {
        var builder = AutomatonBuilder<ShoppingActions, ShoppingState>.Create();
        var shoppingState = builder.Start;

        shoppingState
            .LinkFixedState(ShoppingActions.AddBread, shoppingState, ShoppingStateReducers.AddBread)
            .LinkFixedState(ShoppingActions.AddButter, shoppingState, ShoppingStateReducers.AddButter)
            .ToNewFixedState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);

        var automatonOrError = builder.Build();
        automatonOrError.AssertNone(error => Assert.AreEqual(StateError.NoAccepted, error));
    }

    [Test]
    public void BuildWithAcceptedUnreachable()
    {
        var builder = AutomatonBuilder<ShoppingActions, ShoppingState>.Create();
        var shoppingState = builder.Start;

        shoppingState
            .ToNewFixedState(ShoppingActions.AddBread, ShoppingStateReducers.AddBread)
            .ToNewFixedState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods)
            .ToNewFixedAccepted(ShoppingActions.ReceiveGoods, ShoppingStateReducers.ReceiveGoods);

        shoppingState
            .ToNewFixedState(ShoppingActions.AddButter, ShoppingStateReducers.AddButter);

        var automatonOrError = builder.Build();
        automatonOrError.AssertNone(error => Assert.AreEqual(StateError.AcceptedIsUnreachable, error));
    }
}