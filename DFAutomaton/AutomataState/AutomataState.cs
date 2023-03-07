using Optional;
using Optional.Collections;

namespace DFAutomaton
{
    public class AutomataState<TTransition, TState> where TTransition : notnull
    {
        private readonly IReadOnlyDictionary<TTransition, AutomataNextState<TTransition, TState>> _nextStates;

        internal AutomataState(
            StateType type,
            IReadOnlyDictionary<TTransition, AutomataNextState<TTransition, TState>> nextStates)
        {
            _nextStates = nextStates;

            Type = type;
            Transitions = new HashSet<TTransition>(nextStates.Keys);
        }

        public StateType Type { get; }

        public IReadOnlySet<TTransition> Transitions { get; }

        public Option<AutomataNextState<TTransition, TState>> this[TTransition transition] =>
            _nextStates.GetValueOrNone(transition);
    }
}