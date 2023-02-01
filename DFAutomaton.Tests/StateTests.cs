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
                state => state.AssertValueEquals(expectedValue),
                () => Assert.Fail($"State is None but expected {expectedValue}"));
        }

        [Test]
        public void TestNoTransit()
        {
            var start = ShoppingStateTree.Create();
            var noneStateOption = start[ShoppingActions.Receive];

            noneStateOption.Match(
                state => Assert.Fail($"State is expected to be None but value is {state.Value}"),
                () => { });
        }
    }
}