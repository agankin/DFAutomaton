using Optional;

namespace DFAutomaton
{
    public class State<TTransition, TState>
        where TTransition : notnull
    {
        private readonly Dictionary<TTransition, StateReducer<TTransition, TState>> _transitions = new();

        internal State(
            StateType type,
            AcceptedStateDict<TTransition, TState> acceptedStates)
        {
            Type = type;
            AcceptedStates = acceptedStates;
        }

        public StateType Type { get; }

        public IReadOnlySet<TTransition> Transitions => new HashSet<TTransition>(_transitions.Keys);

        internal AcceptedStateDict<TTransition, TState> AcceptedStates { get; }

        public Option<StateReducer<TTransition, TState>> this[TTransition transition] =>
            _transitions.TryGetValue(transition, out var stateReducer)
                ? Option.Some(stateReducer)
                : Option.None<StateReducer<TTransition, TState>>();

        internal State<TTransition, TState> ToState(
            TTransition transition,
            State<TTransition, TState> dfaState,
            Func<TState, TState> reducer)
        {
            var (state, _) = _transitions[transition] = new StateReducer<TTransition, TState>(dfaState, reducer);

            return state;
        }
    }
}