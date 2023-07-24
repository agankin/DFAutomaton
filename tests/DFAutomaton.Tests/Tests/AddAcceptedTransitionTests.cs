using NUnit.Framework;
using Optional.Unsafe;

namespace DFAutomaton.Tests;

[TestFixture]
public class AddAcceptedTransitionTests
{
    [Test]
    public void AddNewWithReducer()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newState = start.ToNewFixedAccepted(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);

        start[ShoppingActions.PayForGoods].AssertTransitionToAccepted(ShoppingStateReducers.PayForGoods);
    }

    [Test]
    public void AddNewConstantState()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newValue = new ShoppingState(ShoppingStateType.GoodsPaid, 100);
        start.ToNewFixedAccepted(ShoppingActions.PayForGoods, newValue);

        var initialValue = new ShoppingState(ShoppingStateType.Shopping, 0);
        start[ShoppingActions.PayForGoods].AssertTransitionToAccepted(initialValue, newValue);
    }

    [Test]
    public void LinkExistingStateWithReducer()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newAccepted = start.ToNewFixedAccepted(ShoppingActions.AddBread, ShoppingStateReducers.ReceiveGoods);
        var linkedAccepted = start.LinkFixedAccepted(ShoppingActions.AddButter, newAccepted, ShoppingStateReducers.ReceiveGoods);

        Assert.AreEqual(newAccepted, linkedAccepted);
        start[ShoppingActions.AddButter].AssertTransitionToAccepted(ShoppingStateReducers.ReceiveGoods);

        start[ShoppingActions.AddBread].AssertSome(breadAddedTransition =>
        {
            start[ShoppingActions.AddButter].AssertSome(butterAddedTransition =>
            {
                var (breadAdded, _, breadAddedReduce) = breadAddedTransition;
                var (butterAdded, _, butterAddedReduce) = butterAddedTransition;

                Assert.AreEqual(breadAdded, butterAdded);
                Assert.AreEqual(breadAddedReduce, butterAddedReduce);
            });
        });
    }

    [Test]
    public void LinkExistingConstantState()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newValue = new ShoppingState(ShoppingStateType.GoodsPurchased, 0);
        var newAccepted = start.ToNewFixedAccepted(ShoppingActions.AddBread, newValue);
        var linkedAccepted = start.LinkFixedAccepted(ShoppingActions.AddButter, newAccepted, newValue);

        Assert.AreEqual(newAccepted, linkedAccepted);
        
        var initialValue = new ShoppingState(ShoppingStateType.Shopping, 0);
        start[ShoppingActions.AddButter].AssertTransitionToAccepted(initialValue, newValue);

        start[ShoppingActions.AddBread].AssertSome(addBread =>
            start[ShoppingActions.AddButter].AssertSome(addButter =>
                Assert.AreEqual(addBread.State, addButter.State)));
    }
}