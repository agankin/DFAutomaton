using NUnit.Framework;

namespace DFAutomaton.Tests
{
    [TestFixture]
    public class AutomataStateGraphValidationTests
    {
        [Test]
        public void BuildWithoutAccepted()
        {
            var builder = AutomataBuilder<ShoppingActions, ShoppingState>.Create();
            var shoppingState = builder.StartState;

            shoppingState
                .LinkState(ShoppingActions.AddBread, shoppingState, ShoppingStateReducers.AddBread)
                .LinkState(ShoppingActions.AddButter, shoppingState, ShoppingStateReducers.AddButter)
                .ToNewState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods);

            var automataOrError = builder.Build();
            automataOrError.AssertNone(error => Assert.AreEqual(AutomataGraphError.NoAccepted, error));
        }

        [Test]
        public void BuildWithAcceptedUnreachable()
        {
            var builder = AutomataBuilder<ShoppingActions, ShoppingState>.Create();
            var shoppingState = builder.StartState;

            shoppingState
                .ToNewState(ShoppingActions.AddBread, ShoppingStateReducers.AddBread)
                .ToNewState(ShoppingActions.PayForGoods, ShoppingStateReducers.PayForGoods)
                .ToNewAccepted(ShoppingActions.ReceiveGoods, ShoppingStateReducers.ReceiveGoods);

            shoppingState
                .ToNewState(ShoppingActions.AddButter, ShoppingStateReducers.AddButter);

            var automataOrError = builder.Build();
            automataOrError.AssertNone(error => Assert.AreEqual(AutomataGraphError.AcceptedIsUnreachable, error));
        }
    }
}