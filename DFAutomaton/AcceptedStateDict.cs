using Optional;
using Optional.Collections;

namespace DFAutomaton
{
    internal class AcceptedStateDict<TTransition, TState>
        where TTransition : notnull
    {
        private readonly Dictionary<AcceptedStateHandle<TState>, NextState<TTransition, TState>> _acceptedStates = new();

        public Option<NextState<TTransition, TState>> this[AcceptedStateHandle<TState> acceptedState] =>
            _acceptedStates.GetValueOrNone(acceptedState);

        public void Add(
            AcceptedStateHandle<TState> acceptedState,
            State<TTransition, TState> state,
            Func<TState, TState> reducer)
        {
            var nextState = new NextState<TTransition, TState>(state, reducer);

            _acceptedStates.Add(acceptedState, nextState);
        }
    }
}