using NUnit.Framework;

namespace DFAutomaton.Tests
{
    [TestFixture]
    public class StateTests
    {
        [Test]
        public void TestTransit()
        {
            var start = ShoppingStateTree.Create();
            var orderedStateOption = start[ShoppingActions.Order];
            
            var expectedValue = ShoppingStates.Ordered;

            orderedStateOption.Match(
                state => state.AssertReducedTo(expectedValue),
                () => Assert.Fail($"Next state must not be None"));
        }

        [Test]
        public void TestNoTransit()
        {
            var start = ShoppingStateTree.Create();
            var noneStateOption = start[ShoppingActions.Receive];

            noneStateOption.Match(
                state => Assert.Fail($"Next state must be None"),
                () => { });
        }
    }
}