using NUnit.Framework;

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

        var afterAddButter = afterAddBread.FlatMap(state => state.NextState[ShoppingActions.AddButter]);
        afterAddButter.AssertTransition(shopping, ShoppingStateReducers.AddButter);

        var afterPay = afterAddButter.FlatMap(state => state.NextState[ShoppingActions.PayForGoods]);
        afterPay.AssertTransition(paid, ShoppingStateReducers.PayForGoods);

        var afterReceive = afterPay.FlatMap(state => state.NextState[ShoppingActions.ReceiveGoods]);
        afterReceive.AssertSome(nextState =>
        {
            Assert.AreEqual(StateType.Accepted, nextState.NextState.Type);
            Assert.AreEqual(ShoppingStateReducers.ReceiveGoods, nextState.Reducer);
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