using NUnit.Framework;

namespace DFAutomaton.Tests
{
    [TestFixture]
    public class AutomataStateTests
    {
        [Test]
        public void TestTransition()
        {
            var shoppingState = ShoppingStateTree.Create().ShoppingState.BuildAutomataTree();

            var afterAddBread = shoppingState[ShoppingActions.AddBread];
            afterAddBread.AssertSome(state => Assert.AreEqual(shoppingState, state.State));

            var afterAddButter = afterAddBread.FlatMap(state => state.State[ShoppingActions.AddButter]);
            afterAddButter.AssertSome(state => Assert.AreEqual(shoppingState, state.State));

            var paid = afterAddButter.FlatMap(state => state.State[ShoppingActions.PayForGoods]);
            paid.AssertSome();
        }

        [Test]
        public void TestNoTransition()
        {
            var startState = ShoppingStateTree.Create().ShoppingState.BuildAutomataTree();
            var receive = startState[ShoppingActions.ReceiveGoods];

            receive.AssertNone();
        }
    }
}