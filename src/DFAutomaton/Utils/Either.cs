namespace DFAutomaton;

/// <summary>
/// Contains either one of two possible values.
/// </summary>
/// <typeparam name="T1">First value type.</typeparam>
/// <typeparam name="T2">Second value type.</typeparam>
public class Either<T1, T2>
{
    private readonly T1 _first = default!;
    private readonly T2 _second = default!;
    private readonly bool _isFirst;

    public Either(T1 first)
    {
        _first = first;
        _isFirst = true;
    }

    public Either(T2 second)
    {
        _second = second;
        _isFirst = false;
    }

    /// <summary>
    /// Matches either one of two possible values by invoking corresponding delegate.
    /// </summary>
    /// <param name="onFirst">A delegate invoked for first value.</param>
    /// <param name="onSecond">A delegate invoked for second value.</param>
    public void Match(Action<T1> onFirst, Action<T2> onSecond)
    {
        if (_isFirst)
            onFirst(_first);
        else
            onSecond(_second);
    }

    /// <summary>
    /// Matches either one of two possible values by invoking corresponding delegate.
    /// </summary>
    /// <param name="onFirst">A delegate invoked for first value.</param>
    /// <param name="onSecond">A delegate invoked for second value.</param>
    /// <returns>A value returned from the invoked delegate.</returns>
    public TResult Match<TResult>(Func<T1, TResult> onFirst, Func<T2, TResult> onSecond)
    {
        return _isFirst ? onFirst(_first) : onSecond(_second);
    }

    public static implicit operator Either<T1, T2>(T1 value) => new(value);

    public static implicit operator Either<T1, T2>(T2 value) => new(value);
}