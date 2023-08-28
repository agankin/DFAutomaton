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
        var newState = start.ToAccepted(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);

        start[ShoppingActions.PayForGoods].AssertTransitionToAccepted(ShoppingStateReducers.PayForGoods);
    }

    [Test]
    public void AddNewConstantState()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newValue = new ShoppingState(ShoppingStateType.GoodsPaid, 100);
        start.ToAccepted(ShoppingActions.PayForGoods, newValue);

        var initialValue = new ShoppingState(ShoppingStateType.Shopping, 0);
        start[ShoppingActions.PayForGoods].AssertTransitionToAccepted(initialValue, newValue);
    }
}