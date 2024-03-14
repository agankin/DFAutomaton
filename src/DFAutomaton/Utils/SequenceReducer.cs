namespace DFAutomaton.Utils;

internal class SequenceAggregator<TValue, TResult>
{
    private readonly Predicate<TResult> _isErrorResult;

    public SequenceAggregator(Predicate<TResult> isErrorResult)
    {
        _isErrorResult = isErrorResult;
    }

    public TResult Reduce(TResult initial, IEnumerable<TValue> values, Func<TResult, TValue, TResult> reduce)
    {
        var current = initial;
        foreach (var value in values)
        {
            if (_isErrorResult(current))
                return current;

            current = reduce(current, value);
        }

        return current;
    }
}