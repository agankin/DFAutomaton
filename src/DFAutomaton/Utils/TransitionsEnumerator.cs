using System.Collections;
using Optional;

namespace DFAutomaton.Utils
{
    internal class TransitionsEnumerator<TTransition> : IEnumerator<TTransition>
    {
        private readonly IEnumerator<TTransition> _transitionsEnumerator;
        private readonly Queue<TTransition> _emitedQueue = new Queue<TTransition>();

        private Option<TTransition> _current = Option.None<TTransition>();

        public TransitionsEnumerator(IEnumerable<TTransition> transitions)
        {
            _transitionsEnumerator = transitions.GetEnumerator();
        }

        public TTransition Current => _current.ValueOr(() => throw new InvalidOperationException());

        object? IEnumerator.Current => Current;

        public void QueueEmited(TTransition transition) => _emitedQueue.Enqueue(transition);

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

        public void Reset() => throw new NotSupportedException();

        public IEnumerable<TTransition> ToEnumerable() => new EnumerableWrapper(this);

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
}