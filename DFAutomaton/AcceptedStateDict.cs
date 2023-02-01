using DFAutomaton.Extensions;
using Optional;

namespace DFAutomaton
{
    internal class AcceptedStateDict<TTransition, TState>
        where TTransition : notnull
    {
        private readonly Dictionary<AcceptedState<TState>, State<TTransition, TState>> _acceptedStates = new();

        public Option<State<TTransition, TState>> this[AcceptedState<TState> acceptedState] =>
            _acceptedStates.Get(acceptedState);

        public void Add(AcceptedState<TState> acceptedState, State<TTransition, TState> state) =>
            _acceptedStates.Add(acceptedState, state);
    }
}