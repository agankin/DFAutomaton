using NUnit.Framework;

namespace DFAutomaton.Tests
{
    public static class AssertExtensions
    {
        public static TValue ItIs<TValue>(this TValue value, TValue expectedValue)
        {
            Assert.That(value, Is.EqualTo(expectedValue));
            return value;
        }
    }
}