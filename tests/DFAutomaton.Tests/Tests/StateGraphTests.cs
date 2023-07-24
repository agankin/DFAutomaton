﻿using NUnit.Framework;

namespace DFAutomaton.Tests;

[TestFixture]
public class StateGraphTests
{
    [Test]
    public void TransitionsScenario()
    {
        var graph = ShoppingStateGraph.Create();

        var shopping = graph.ShoppingState;
        var paid = graph.PaidState;

        var afterAddBread = shopping[ShoppingActions.AddBread];
        afterAddBread.AssertTransition(shopping, ShoppingStateReducers.AddBread);

        var afterAddButter = afterAddBread.FlatMap(state => state.State).FlatMap(nextState => nextState[ShoppingActions.AddButter]);
        afterAddButter.AssertTransition(shopping, ShoppingStateReducers.AddButter);

        var afterPay = afterAddButter.FlatMap(state => state.State).FlatMap(nextState => nextState[ShoppingActions.PayForGoods]);
        afterPay.AssertTransition(paid, ShoppingStateReducers.PayForGoods);

        var afterReceive = afterPay.FlatMap(state => state.State).FlatMap(nextState => nextState[ShoppingActions.ReceiveGoods]);
        afterReceive.AssertSome(transition =>
        {
            var (_, nextStateOption, reduce) = transition;

            nextStateOption.AssertSome(nextState => Assert.AreEqual(StateType.Accepted, nextState.Type));
            Assert.AreEqual(ShoppingStateReducers.ReceiveGoods, reduce);
        });
    }

    [Test]
    public void TestTransitionNotExists()
    {
        var graph = ShoppingStateGraph.Create();
        var goodsReceivedState = graph.ShoppingState[ShoppingActions.ReceiveGoods];

        goodsReceivedState.AssertNone();
    }
}