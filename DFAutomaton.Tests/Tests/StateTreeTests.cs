using NUnit.Framework;

namespace DFAutomaton.Tests
{
    [TestFixture]
    public class StateTreeTests
    {
        [Test]
        public void TransitionsScenario()
        {
            var tree = ShoppingStateTree.Create();

            var shopping = tree.ShoppingState;
            var paid = tree.PaidState;

            var afterAddBread = shopping[ShoppingActions.AddBread];
            afterAddBread.AssertSomeNextState(shopping, ShoppingStateReducers.AddBread);

            var afterAddButter = afterAddBread.FlatMap(state => state.State[ShoppingActions.AddButter]);
            afterAddButter.AssertSomeNextState(shopping, ShoppingStateReducers.AddButter);

            var afterPay = afterAddButter.FlatMap(state => state.State[ShoppingActions.PayForGoods]);
            afterPay.AssertSomeNextState(paid, ShoppingStateReducers.PayForGoods);

            var afterReceive = afterPay.FlatMap(state => state.State[ShoppingActions.ReceiveGoods]);
            afterReceive.AssertSome(nextState =>
            {
                Assert.AreEqual(StateType.Accepted, nextState.State.Type);
                Assert.AreEqual(ShoppingStateReducers.ReceiveGoods, nextState.Reducer);
            });
        }

        [Test]
        public void TestTransitionNotExists()
        {
            var tree = ShoppingStateTree.Create();
            var goodsReceivedState = tree.ShoppingState[ShoppingActions.ReceiveGoods];

            goodsReceivedState.AssertNone();
        }
    }
}