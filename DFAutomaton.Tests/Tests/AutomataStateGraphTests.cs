using NUnit.Framework;
using Optional.Unsafe;

namespace DFAutomaton.Tests
{
    [TestFixture]
    public class AutomataStateGraphTests
    {
        [Test]
        public void TransitionsScenario()
        {
            var graph = ShoppingStateGraph.Create();
            var start = graph.ShoppingState.BuildAutomataGraph().AssertSome();

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
        }

        [Test]
        public void TestTransitionNotExists()
        {
            var stateGraph = ShoppingStateGraph.Create();
            var start = stateGraph.ShoppingState.BuildAutomataGraph().AssertSome();
            var receive = start[ShoppingActions.ReceiveGoods];

            receive.AssertNone();
        }
    }
}