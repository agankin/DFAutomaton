using NUnit.Framework;

namespace DFAutomaton.Tests;

[TestFixture]
public class AddTransitionTests
{
    [Test]
    public void AddNewStateWithReducer()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newState = start.ToNewFixedState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);

        Assert.AreEqual(StateType.SubState, newState.Type);
        start[ShoppingActions.PayForGoods].AssertTransition(newState, ShoppingStateReducers.PayForGoods);
    }

    [Test]
    public void AddNewConstantState()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newValue = new ShoppingState(ShoppingStateType.GoodsPaid, 100);
        var newState = start.ToNewFixedState(ShoppingActions.PayForGoods, newValue);

        Assert.AreEqual(StateType.SubState, newState.Type);

        var initialValue = new ShoppingState(ShoppingStateType.Shopping, 0);
        start[ShoppingActions.PayForGoods].AssertTransition(newState, initialValue, newValue);
    }

    [Test]
    public void LinkExistingStateWithReducer()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newState = StateFactory<ShoppingActions, ShoppingState>.SubState(start.GenerateId);
        
        var newLinkedState = start.LinkFixedState(
            ShoppingActions.AddBread,
            newState,
            ShoppingStateReducers.AddBread);

        Assert.AreEqual(newState, newLinkedState);
        start[ShoppingActions.AddBread].AssertTransition(newState, ShoppingStateReducers.AddBread);
    }

    [Test]
    public void LinkExistingConstantState()
    {
        var start = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newState = StateFactory<ShoppingActions, ShoppingState>.SubState(start.GenerateId);
        var newValue = new ShoppingState(ShoppingStateType.Shopping, ShoppingStateReducers.BreadPrice);
        
        var newLinkedState = start.LinkFixedState(ShoppingActions.AddBread, newState, newValue);

        Assert.AreEqual(newState, newLinkedState);

        var initialValue = new ShoppingState(ShoppingStateType.Shopping, 0);
        start[ShoppingActions.AddBread].AssertTransition(newState, initialValue, newValue);
    }
}