using NUnit.Framework;

namespace DFAutomaton.Tests
{
    [TestFixture]
    public class AutomatonStateGraphTests
    {
        [Test]
        public void TransitionsScenario()
        {
            var graph = ShoppingStateGraph.Create();

            graph.ShoppingState.BuildAutomatonGraph().AssertSome(start =>
            {
                var afterAddBread = start[ShoppingActions.AddBread];
                afterAddBread.AssertTransition(start, ShoppingStateReducers.AddBread);

                var afterAddButter = afterAddBread.FlatMap(state => state.NextState[ShoppingActions.AddButter]);
                afterAddButter.AssertTransition(start, ShoppingStateReducers.AddButter);

                var afterPaid = afterAddButter.FlatMap(state => state.NextState[ShoppingActions.PayForGoods]);
                afterPaid.AssertSome(nextState =>
                {
                    Assert.AreEqual(StateType.SubState, nextState.NextState.Type);
                    Assert.AreEqual(ShoppingStateReducers.PayForGoods, nextState.Reducer);
                });

                var afterReceived = afterPaid.FlatMap(state => state.NextState[ShoppingActions.ReceiveGoods]);
                afterReceived.AssertSome(nextState =>
                {
                    Assert.AreEqual(StateType.Accepted, nextState.NextState.Type);
                    Assert.AreEqual(ShoppingStateReducers.ReceiveGoods, nextState.Reducer);
                });
            });
        }

        [Test]
        public void TestTransitionNotExists()
        {
            var stateGraph = ShoppingStateGraph.Create();

            stateGraph.ShoppingState.BuildAutomatonGraph().AssertSome(start =>
            {
                var receive = start[ShoppingActions.ReceiveGoods];
                receive.AssertNone();
            });
        }
    }
}