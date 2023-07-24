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
            afterAddBread.AssertTransition(start, ShoppingStateReducers.AddBread);

            var afterAddButter = afterAddBread.FlatMap(state => state.State).FlatMap(nextState => nextState[ShoppingActions.AddButter]);
            afterAddButter.AssertTransition(start, ShoppingStateReducers.AddButter);

            var afterPaid = afterAddButter.FlatMap(state => state.State).FlatMap(nextState => nextState[ShoppingActions.PayForGoods]);
            afterPaid.AssertSome(transition =>
            {
                var (_, nextStateOption, reduce) = transition;

                nextStateOption.AssertSome(nextState => Assert.AreEqual(StateType.SubState, nextState.Type));
                Assert.AreEqual(ShoppingStateReducers.PayForGoods, reduce);
            });

            var afterReceived = afterPaid.FlatMap(state => state.State).FlatMap(nextState => nextState[ShoppingActions.ReceiveGoods]);
            afterReceived.AssertSome(transition =>
            {
                var (_, nextStateOption, reduce) = transition;

                nextStateOption.AssertSome(nextState => Assert.AreEqual(StateType.Accepted, nextState.Type));
                Assert.AreEqual(ShoppingStateReducers.ReceiveGoods, reduce);
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