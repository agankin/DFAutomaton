using Optional;
using Optional.Collections;

namespace DFAutomaton
{
    internal class AutomataState<TTransition, TState> : IState<TTransition, TState>
        where TTransition : notnull
    {
        private readonly IReadOnlyDictionary<TTransition, Next<TTransition, TState>> _nextStates;

        internal AutomataState(
            long id,
            object? tag,
            StateType type,
            IReadOnlyDictionary<TTransition, Next<TTransition, TState>> nextStates)
        {
            Id = id;
            Tag = tag;
            Type = type;
            _nextStates = nextStates;
        }

        public long Id { get; }

        public object? Tag { get; }

        public StateType Type { get; }

        public IReadOnlySet<TTransition> Transitions => new HashSet<TTransition>(_nextStates.Keys);

        public Option<Next<TTransition, TState>> this[TTransition transition] =>
            _nextStates.GetValueOrNone(transition);

        public override string? ToString() => this.Format();
    }
}