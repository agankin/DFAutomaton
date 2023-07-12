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
            .LinkState(ShoppingActions.AddBread, shoppingState, ShoppingStateReducers.AddBread)
            .LinkState(ShoppingActions.AddButter, shoppingState, ShoppingStateReducers.AddButter)
            .ToNewState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);

        var automatonOrError = builder.Build();
        automatonOrError.AssertNone(error => Assert.AreEqual(StateError.NoAccepted, error));
    }

    [Test]
    public void BuildWithAcceptedUnreachable()
    {
        var builder = AutomatonBuilder<ShoppingActions, ShoppingState>.Create();
        var shoppingState = builder.Start;

        shoppingState
            .ToNewState(ShoppingActions.AddBread, ShoppingStateReducers.AddBread)
            .ToNewState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods)
            .ToNewAccepted(ShoppingActions.ReceiveGoods, ShoppingStateReducers.ReceiveGoods);

        shoppingState
            .ToNewState(ShoppingActions.AddButter, ShoppingStateReducers.AddButter);

        var automatonOrError = builder.Build();
        automatonOrError.AssertNone(error => Assert.AreEqual(StateError.AcceptedIsUnreachable, error));
    }
}