using Optional;
using Optional.Collections;

namespace DFAutomaton
{
    public class AutomataState<TTransition, TState> where TTransition : notnull
    {
        private readonly IReadOnlyDictionary<TTransition, AutomataNextState<TTransition, TState>> _nextStates;

        internal AutomataState(
            object? tag,
            StateType type,
            IReadOnlyDictionary<TTransition, AutomataNextState<TTransition, TState>> nextStates)
        {
            Tag = tag;
            Type = type;
            _nextStates = nextStates;
        }

        public object? Tag { get; }

        public StateType Type { get; }

        public IReadOnlySet<TTransition> Transitions => new HashSet<TTransition>(_nextStates.Keys);

        public Option<AutomataNextState<TTransition, TState>> this[TTransition transition] =>
            _nextStates.GetValueOrNone(transition);

        public override string? ToString() => Tag?.ToString() ?? base.ToString();
    }
}