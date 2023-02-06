using DFAutomaton.Extensions;
using Optional;

namespace DFAutomaton
{
    internal class AcceptedStateDict<TTransition, TState>
        where TTransition : notnull
    {
        private readonly Dictionary<AcceptedState<TState>, StateReducer<TTransition, TState>> _acceptedStates = new();

        public Option<StateReducer<TTransition, TState>> this[AcceptedState<TState> acceptedState] =>
            _acceptedStates.Get(acceptedState);

        public void Add(
            AcceptedState<TState> acceptedState,
            State<TTransition, TState> state,
            Func<TState, TState> reducer)
        {
            var stateReducer = new StateReducer<TTransition, TState>(state, reducer);

            _acceptedStates.Add(acceptedState, stateReducer);
        }
    }
}