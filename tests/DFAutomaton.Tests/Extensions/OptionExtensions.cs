using NUnit.Framework;
using PureMonads;

namespace DFAutomaton.Tests;

public static class OptionExtensions
{
    public static TValue IsSome<TValue>(this Option<TValue> option) =>
        option.Or(() =>
        {
            Assert.Fail("Expected Some but optional value is None.");
            throw new Exception();
        });

    public static TValue IsSome<TValue, TError>(this Result<TValue, TError> result) =>
        result.Match(
            _ => _,
            _ =>
            {
                Assert.Fail("Expected Value but the value is Error.");
                throw new Exception();
            });

    public static void IsNone<TValue>(this Option<TValue> option) =>
        option.Match(
            _ =>
            {
                Assert.Fail("Expected None but optional value is Some.");
                return new Nothing();
            },
            () => new Nothing());

    public static TError IsError<TValue, TError>(this Result<TValue, TError> result) =>
        result.Match(
            _ =>
            {
                Assert.Fail("Expected None but optional value is Some.");
                throw new Exception();
            },
            error => error);

    public static void IsError<TValue, TError>(this Result<TValue, TError> result, TError expectedError) =>
        result.Match(
            _ =>
            {
                Assert.Fail("Expected None but optional value is Some.");
                throw new Exception();
            },
            error =>
            {
                error.ItIs(expectedError);
                return new Nothing();
            });
}