using NUnit.Framework;

namespace DFAutomaton.Tests;

[TestFixture]
public class AutomatonStateGraphTests
{
    [Test]
    public void TransitionsScenario()
    {
        var graph = ShoppingStateGraph.Create();

        graph.ShoppingState.Complete(BuildConfiguration.Default).AssertSome(start =>
        {
            var afterAddBread = start[ShoppingActions.AddBread];
            afterAddBread.AssertMove(start, ShoppingStateReducers.AddBread);

            var afterAddButter = afterAddBread.FlatMap(state => state.NextState[ShoppingActions.AddButter]);
            afterAddButter.AssertMove(start, ShoppingStateReducers.AddButter);

            var afterPaid = afterAddButter.FlatMap(state => state.NextState[ShoppingActions.PayForGoods]);
            afterPaid.AssertSome(move =>
            {
                Assert.AreEqual(StateType.SubState, move.NextState.Type);
                Assert.AreEqual(ShoppingStateReducers.PayForGoods, move.Reducer);
            });

            var afterReceived = afterPaid.FlatMap(state => state.NextState[ShoppingActions.ReceiveGoods]);
            afterReceived.AssertSome(move =>
            {
                Assert.AreEqual(StateType.Accepted, move.NextState.Type);
                Assert.AreEqual(ShoppingStateReducers.ReceiveGoods, move.Reducer);
            });
        });
    }

    [Test]
    public void TestTransitionNotExists()
    {
        var stateGraph = ShoppingStateGraph.Create();

        stateGraph.ShoppingState.Complete(BuildConfiguration.Default).AssertSome(start =>
        {
            var receive = start[ShoppingActions.ReceiveGoods];
            receive.AssertNone();
        });
    }
}