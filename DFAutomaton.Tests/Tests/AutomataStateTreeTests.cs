using NUnit.Framework;

namespace DFAutomaton.Tests
{
    [TestFixture]
    public class AutomataStateTreeTests
    {
        [Test]
        public void TransitionsScenario()
        {
            var tree = ShoppingStateTree.Create();
            var start = tree.ShoppingState.BuildAutomataTree();

            var afterAddBread = start[ShoppingActions.AddBread];
            afterAddBread.AssertSomeNextState(start, ShoppingStateReducers.AddBread);

            var afterAddButter = afterAddBread.FlatMap(state => state.State[ShoppingActions.AddButter]);
            afterAddButter.AssertSomeNextState(start, ShoppingStateReducers.AddButter);

            var afterPaid = afterAddButter.FlatMap(state => state.State[ShoppingActions.PayForGoods]);
            afterPaid.AssertSome(nextState =>
            {
                Assert.AreEqual(StateType.SubState, nextState.State.Type);
                Assert.AreEqual(ShoppingStateReducers.PayForGoods, nextState.Reducer);
            });

            var afterReceived = afterPaid.FlatMap(state => state.State[ShoppingActions.ReceiveGoods]);
            afterReceived.AssertSome(nextState =>
            {
                Assert.AreEqual(StateType.Accepted, nextState.State.Type);
                Assert.AreEqual(ShoppingStateReducers.ReceiveGoods, nextState.Reducer);
            });
        }

        [Test]
        public void TestTransitionNotExists()
        {
            var startState = ShoppingStateTree.Create().ShoppingState.BuildAutomataTree();
            var receive = startState[ShoppingActions.ReceiveGoods];

            receive.AssertNone();
        }
    }
}