using NUnit.Framework;

namespace DFAutomaton.Tests;

[TestFixture]
public class AddTransitionTests
{
    [Test]
    public void AddNewStateWithReducer()
    {
        var startState = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newState = startState.ToNewState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);

        Assert.AreEqual(StateType.SubState, newState.Type);
        startState[ShoppingActions.PayForGoods].AssertTransition(
            newState,
            ShoppingStateReducers.PayForGoods);
    }

    [Test]
    public void AddNewConstantState()
    {
        var startState = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newValue = new ShoppingState(ShoppingStateType.GoodsPaid, 100);
        var newState = startState.ToNewState(ShoppingActions.PayForGoods, newValue);

        Assert.AreEqual(StateType.SubState, newState.Type);

        var initialValue = new ShoppingState(ShoppingStateType.Shopping, 0);
        startState[ShoppingActions.PayForGoods]
            .AssertTransition(newState, initialValue, newValue);
    }

    [Test]
    public void LinkExistingStateWithReducer()
    {
        var startState = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newState = StateFactory<ShoppingActions, ShoppingState>.SubState(startState.GetNextId);
        
        var newLinkedState = startState.LinkState(
            ShoppingActions.AddBread,
            newState,
            ShoppingStateReducers.AddBread);

        Assert.AreEqual(newState, newLinkedState);
        startState[ShoppingActions.AddBread]
            .AssertTransition(newState, ShoppingStateReducers.AddBread);
    }

    [Test]
    public void LinkExistingConstantState()
    {
        var startState = StateFactory<ShoppingActions, ShoppingState>.Start();
        var newState = StateFactory<ShoppingActions, ShoppingState>.SubState(startState.GetNextId);
        var newValue = new ShoppingState(ShoppingStateType.Shopping, ShoppingStateReducers.BreadPrice);
        
        var newLinkedState = startState.LinkState(ShoppingActions.AddBread, newState, newValue);

        Assert.AreEqual(newState, newLinkedState);

        var initialValue = new ShoppingState(ShoppingStateType.Shopping, 0);
        startState[ShoppingActions.AddBread]
            .AssertTransition(newState, initialValue, newValue);
    }
}