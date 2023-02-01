using Optional;

namespace DFAutomaton
{
    public class State<TTransition, TState>
        where TTransition : notnull
    {
        private readonly Dictionary<TTransition, State<TTransition, TState>> _transitions = new();

        internal State(
            StateType type,
            Option<TState> value,
            AcceptedStateDict<TTransition, TState> acceptedStates)
        {
            Type = type;
            Value = value;
            AcceptedStates = acceptedStates;
        }

        public StateType Type { get; }

        public Option<TState> Value { get; }

        internal AcceptedStateDict<TTransition, TState> AcceptedStates { get; }

        public Option<State<TTransition, TState>> this[TTransition transition] =>
            _transitions.TryGetValue(transition, out var state)
                ? Option.Some(state)
                : Option.None<State<TTransition, TState>>();

        public State<TTransition, TState> ToState(
            TTransition transition,
            State<TTransition, TState> dfaState)
        {
            return _transitions[transition] = dfaState;
        }
    }
}