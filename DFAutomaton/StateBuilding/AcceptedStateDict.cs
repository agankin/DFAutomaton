using Optional;
using Optional.Collections;

namespace DFAutomaton
{
    internal class AcceptedStateDict<TTransition, TState> where TTransition : notnull
    {
        private readonly Dictionary<AcceptedStateHandle<TState>, State<TTransition, TState>> _acceptedStates = new();

        public Option<State<TTransition, TState>> this[AcceptedStateHandle<TState> acceptedState] =>
            _acceptedStates.GetValueOrNone(acceptedState);

        public void Add(AcceptedStateHandle<TState> acceptedStateHandle, State<TTransition, TState> acceptedState) =>
            _acceptedStates.Add(acceptedStateHandle, acceptedState);
    }
}