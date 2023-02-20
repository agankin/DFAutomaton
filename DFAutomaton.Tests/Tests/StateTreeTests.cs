using NUnit.Framework;

namespace DFAutomaton.Tests
{
    [TestFixture]
    public class StateTreeTests
    {
        [Test]
        public void TestTransition()
        {
            var tree = ShoppingStateTree.Create();

            var afterAddBread = tree.ShoppingState[ShoppingActions.AddBread];
            afterAddBread.AssertSome(state => Assert.AreEqual(tree.ShoppingState, state.State));

            var afterAddButter = afterAddBread.FlatMap(state => state.State[ShoppingActions.AddButter]);
            afterAddButter.AssertSome(state => Assert.AreEqual(tree.ShoppingState, state.State));

            var paid = afterAddButter.FlatMap(state => state.State[ShoppingActions.PayForGoods]);
            paid.AssertSome(paidState => Assert.True(tree.IsGoodsPaidState(paidState.State)));
        }

        [Test]
        public void TestNoTransition()
        {
            var tree = ShoppingStateTree.Create();
            var receive = tree.ShoppingState[ShoppingActions.ReceiveGoods];

            receive.AssertNone();
        }
    }
}