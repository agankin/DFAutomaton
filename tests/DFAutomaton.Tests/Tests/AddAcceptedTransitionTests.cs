using NUnit.Framework;

namespace DFAutomaton.Tests;

[TestFixture]
public class AddAcceptedTransitionTests
{
    [Test]
    public void AddNewWithReducer()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newState = start.ToNewAccepted(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);

        start[ShoppingActions.PayForGoods].AssertAccepted(ShoppingStateReducers.PayForGoods);
    }

    [Test]
    public void AddNewConstantState()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newValue = new ShoppingState(ShoppingStateType.GoodsPaid, 100);
        start.ToNewAccepted(ShoppingActions.PayForGoods, newValue);

        var initialValue = new ShoppingState(ShoppingStateType.Shopping, 0);
        start[ShoppingActions.PayForGoods].AssertAccepted(initialValue, newValue);
    }

    [Test]
    public void LinkExistingStateWithReducer()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newAccepted = start.ToNewAccepted(ShoppingActions.AddBread, ShoppingStateReducers.ReceiveGoods);
        var linkedAccepted = start.LinkAccepted(ShoppingActions.AddButter, newAccepted, ShoppingStateReducers.ReceiveGoods);

        Assert.AreEqual(newAccepted, linkedAccepted);
        start[ShoppingActions.AddButter].AssertAccepted(ShoppingStateReducers.ReceiveGoods);
        Assert.AreEqual(start[ShoppingActions.AddBread], start[ShoppingActions.AddButter]);
    }

    [Test]
    public void LinkExistingConstantState()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newValue = new ShoppingState(ShoppingStateType.GoodsPurchased, 0);
        var newAccepted = start.ToNewAccepted(ShoppingActions.AddBread, newValue);
        var linkedAccepted = start.LinkAccepted(ShoppingActions.AddButter, newAccepted, newValue);

        Assert.AreEqual(newAccepted, linkedAccepted);
        
        var initialValue = new ShoppingState(ShoppingStateType.Shopping, 0);
        start[ShoppingActions.AddButter].AssertAccepted(initialValue, newValue);

        start[ShoppingActions.AddBread].AssertSome(addBread =>
            start[ShoppingActions.AddButter].AssertSome(addButter =>
                Assert.AreEqual(addBread.NextState, addButter.NextState)));
    }
}