using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests;

public static class OptionExtensions
{
    public static TValue IsSome<TValue>(this Option<TValue> option) =>
        option.ValueOr(() =>
        {
            Assert.Fail("Expected Some but optional value is None.");
            throw new Exception();
        });

    public static TValue IsSome<TValue, TError>(this Option<TValue, TError> option) =>
        option.ValueOr(() =>
        {
            Assert.Fail("Expected Some but value is Error.");
            throw new Exception();
        });

    public static void IsNone<TValue>(this Option<TValue> option) =>
        option.Match(
            _ => Assert.Fail("Expected None but optional value is Some."),
            () => { });

    public static void IsError<TValue, TError>(this Option<TValue, TError> option, Action<TError> onError) =>
        option.Match(
            _ => Assert.Fail("Expected None but optional value is Some."),
            onError);
}