using NUnit.Framework;

namespace DFAutomaton.Tests
{
    [TestFixture]
    public class ToAcceptedStateTests
    {
        [Test]
        public void ToNewWithReducer()
        {
            var startState = StateFactory<ShoppingActions, ShoppingState>.Start();
            var newStateHandle = startState.ToNewAccepted(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);

            startState[ShoppingActions.PayForGoods].AssertSomeAccepted(ShoppingStateReducers.PayForGoods);
        }

        [Test]
        public void ToNewConstantState()
        {
            var startState = StateFactory<ShoppingActions, ShoppingState>.Start();
            var newValue = new ShoppingState(ShoppingStateType.GoodsPaid, 100);
            startState.ToNewAccepted(ShoppingActions.PayForGoods, newValue);

            var initialValue = new ShoppingState(ShoppingStateType.Shopping, 0);
            startState[ShoppingActions.PayForGoods].AssertSomeAccepted(initialValue, newValue);
        }

        [Test]
        public void LinkExistingStateWithReducer()
        {
            var startState = StateFactory<ShoppingActions, ShoppingState>.Start();
            var newHandle = startState.ToNewAccepted(ShoppingActions.AddBread, ShoppingStateReducers.ReceiveGoods);
            var linkedHandle = startState.LinkAccepted(ShoppingActions.AddButter, newHandle, ShoppingStateReducers.ReceiveGoods);

            Assert.AreEqual(newHandle, linkedHandle);
            startState[ShoppingActions.AddButter].AssertSomeAccepted(ShoppingStateReducers.ReceiveGoods);
            Assert.AreEqual(startState[ShoppingActions.AddBread], startState[ShoppingActions.AddButter]);
        }

        [Test]
        public void LinkExistingConstantState()
        {
            var startState = StateFactory<ShoppingActions, ShoppingState>.Start();
            var newValue = new ShoppingState(ShoppingStateType.GoodsPurchased, 0);
            var newHandle = startState.ToNewAccepted(ShoppingActions.AddBread, newValue);
            var linkedHandle = startState.LinkAccepted(ShoppingActions.AddButter, newHandle, newValue);

            Assert.AreEqual(newHandle, linkedHandle);
            
            var initialValue = new ShoppingState(ShoppingStateType.Shopping, 0);
            startState[ShoppingActions.AddButter].AssertSomeAccepted(initialValue, newValue);

            startState[ShoppingActions.AddBread].AssertSome(addBread =>
                startState[ShoppingActions.AddButter].AssertSome(addButter =>
                    Assert.AreEqual(addBread.State, addButter.State)));
        }
    }
}