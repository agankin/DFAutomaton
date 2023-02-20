namespace DFAutomaton
{
    public static class StateTreeBuilder
    {
        public static State<TTransition, TState> ToState<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            TState state)
            where TTransition : notnull
        {
            return current.ToState(transition, ConstantReducer(state));
        }

        public static State<TTransition, TState> ToState<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            Func<TState, TState> nextState)
            where TTransition : notnull
        {
            var dfaState = StateFactory<TTransition, TState>.SubState(current.AcceptedStates);

            return current.ToState(transition, dfaState, nextState);
        }

        public static AcceptedStateHandle<TState> ToAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            TState state)
            where TTransition : notnull
        {
            var acceptedStates = current.AcceptedStates;

            var dfaState = StateFactory<TTransition, TState>.Accepted(acceptedStates);
            var reducer = ConstantReducer(state);

            current.ToState(transition, dfaState, reducer);

            var acceptedState = new AcceptedStateHandle<TState>();
            acceptedStates.Add(acceptedState, dfaState, reducer);

            return acceptedState;
        }

        public static AcceptedStateHandle<TState> ToAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            Func<TState, TState> reducer)
            where TTransition : notnull
        {
            var acceptedStates = current.AcceptedStates;

            var dfaState = StateFactory<TTransition, TState>.Accepted(acceptedStates);

            current.ToState(transition, dfaState, reducer);

            var acceptedState = new AcceptedStateHandle<TState>();
            acceptedStates.Add(acceptedState, dfaState, reducer);

            return acceptedState;
        }

        public static AcceptedStateHandle<TState> ToAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            AcceptedStateHandle<TState> acceptedState)
            where TTransition : notnull
        {
            var acceptedStates = current.AcceptedStates;
            var nextStateOpt = acceptedStates[acceptedState];

            return nextStateOpt.Match(
                nextState =>
                {
                    var (state, reducer) = nextState;

                    current.ToState(transition, state, reducer);
                    return acceptedState;
                },
                () => acceptedState);
        }

        private static Func<TValue, TValue> ConstantReducer<TValue>(TValue value) => _ => value;
    }
}