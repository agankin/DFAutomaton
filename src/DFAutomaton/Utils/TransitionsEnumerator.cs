using System.Collections;
using Optional;

namespace DFAutomaton.Utils;

/// <summary>
/// The enumerator for transitions with possibility to push new transition ahead.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
internal class TransitionsEnumerator<TTransition> : IEnumerator<TTransition>
{
    private readonly IEnumerator<TTransition> _transitionsEnumerator;
    private readonly Queue<TTransition> _emitedQueue = new();

    private Option<TTransition> _current = Option.None<TTransition>();

    public TransitionsEnumerator(IEnumerable<TTransition> transitions)
    {
        _transitionsEnumerator = transitions.GetEnumerator();
    }

    /// <inheritdoc/>
    public TTransition Current => _current.ValueOr(() => throw new InvalidOperationException());
    
    /// <inheritdoc/>
    object? IEnumerator.Current => Current;

    /// <summary>
    /// Pushes a transition value ahead of a sequence of next transitions to be returned first.
    /// </summary>
    /// <param name="transition">Transition value.</param>
    public void Push(TTransition transition) => _emitedQueue.Enqueue(transition);

    /// <inheritdoc/>
    public bool MoveNext()
    {
        if (_emitedQueue.TryDequeue(out var transition))
        {
            _current = transition.Some();
            return true;
        }

        var hasCurrent = _transitionsEnumerator.MoveNext();
        _current = hasCurrent
            ? _transitionsEnumerator.Current.Some()
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