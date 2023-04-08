using NUnit.Framework;

namespace DFAutomaton.Tests
{
    [TestFixture]
    public class AutomatonTests
    {
        [Test]
        public void RunScenario()
        {
            var automaton = ShoppingStateGraph.Create().Automaton;

            var startState = new ShoppingState(ShoppingStateType.Shopping, 0);
            var transitions = new[]
            {
                ShoppingActions.AddBread,
                ShoppingActions.AddButter,
                ShoppingActions.PayForGoods,
                ShoppingActions.ReceiveGoods
            };

            var expectedCost = ShoppingStateReducers.BreadPrice + ShoppingStateReducers.ButterPrice;
            automaton.Run(startState, transitions).AssertSome(finalState =>
            {
                Assert.AreEqual(expectedCost, finalState.GoodsCost);
                Assert.AreEqual(ShoppingStateType.GoodsPurchased, finalState.Type);
            });
        }

        [Test]
        public void RunWithTransitionNotFoundError()
        {
            var automaton = ShoppingStateGraph.Create().Automaton;

            var startState = new ShoppingState(ShoppingStateType.Shopping, 0);
            var transitions = new[]
            {
                ShoppingActions.AddBread,
                ShoppingActions.AddButter,
                ShoppingActions.ReceiveGoods
            };

            automaton.Run(startState, transitions).AssertNone(error =>
            {
                Assert.AreEqual(AutomatonErrorType.TransitionNotExists, error.Type);
                Assert.AreEqual(ShoppingActions.ReceiveGoods, error.Transition);
            });
        }

        [Test]
        public void RunWithTransitFromAcceptedError()
        {
            var automaton = ShoppingStateGraph.Create().Automaton;

            var startState = new ShoppingState(ShoppingStateType.Shopping, 0);
            var transitions = new[]
            {
                ShoppingActions.AddBread,
                ShoppingActions.AddButter,
                ShoppingActions.PayForGoods,
                ShoppingActions.ReceiveGoods,

                ShoppingActions.PayForGoods
            };

            automaton.Run(startState, transitions).AssertNone(error =>
            {
                Assert.AreEqual(AutomatonErrorType.TransitionFromAccepted, error.Type);
                Assert.AreEqual(ShoppingActions.PayForGoods, error.Transition);
            });
        }
    }
}