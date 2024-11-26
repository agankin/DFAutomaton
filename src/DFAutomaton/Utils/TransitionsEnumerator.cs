using PureMonads;
using System.Collections;

namespace DFAutomaton.Utils;

/// <summary>
/// An enumerator for transitions.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
internal class TransitionsEnumerator<TTransition> : IEnumerator<TTransition>
{
    private readonly IEnumerator<TTransition> _transitionsEnumerator;
    private readonly Queue<TTransition> _yieldedQueue = new();

    private Option<TTransition> _current = Option.None<TTransition>();

    public TransitionsEnumerator(IEnumerable<TTransition> transitions)
    {
        _transitionsEnumerator = transitions.GetEnumerator();
    }

    /// <inheritdoc/>
    public TTransition Current => _current.Or(() => throw new InvalidOperationException());
    
    /// <inheritdoc/>
    object? IEnumerator.Current => Current;

    /// <summary>
    /// Yields the provided transition value as next enumerated value.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    /// <remarks>
    /// The yielded value will be enumerated before initially provided transition values but after other previously yielded values.
    /// </remarks>
    public void YieldNext(TTransition transition) => _yieldedQueue.Enqueue(transition);

    /// <inheritdoc/>
    public bool MoveNext()
    {
        if (_yieldedQueue.TryDequeue(out var transition))
        {
            _current = transition;
            return true;
        }

        var hasCurrent = _transitionsEnumerator.MoveNext();
        _current = hasCurrent
            ? _transitionsEnumerator.Current
            : Option.None<TTransition>();

        return hasCurrent;
    }

    /// <inheritdoc/>
    public void Reset() => throw new NotSupportedException();

    /// <inheritdoc/>
    public IEnumerable<TTransition> ToEnumerable() => new EnumerableWrapper(this);

    /// <inheritdoc/>
    public void Dispose() => _transitionsEnumerator.Dispose();

    private class EnumerableWrapper : IEnumerable<TTransition>
    {
        private readonly IEnumerator<TTransition> _transitionsEnumerator;

        public EnumerableWrapper(IEnumerator<TTransition> transitionsEnumerator)
        {
            _transitionsEnumerator = transitionsEnumerator;
        }

        public IEnumerator<TTransition> GetEnumerator() => _transitionsEnumerator;

        IEnumerator IEnumerable.GetEnumerator() => _transitionsEnumerator;
    }
}