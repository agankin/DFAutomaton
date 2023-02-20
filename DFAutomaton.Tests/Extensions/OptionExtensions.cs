using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests
{
    public static class OptionExtensions
    {
        public static void AssertSome<TValue>(this Option<TValue> option) =>
            AssertSome(option, _ => { });

        public static void AssertSome<TValue>(this Option<TValue> option, Action<TValue> onSome) =>
            option.Match(
                onSome,
                () => Assert.Fail("Expected Some but optional value is None."));

        public static void AssertNone<TValue>(this Option<TValue> option) =>
            option.Match(
                _ => Assert.Fail("Expected None but optional value is Some."),
                () => { });
    }
}