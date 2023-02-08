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
            return current.ToState(transition, FConstant(state));
        }

        public static State<TTransition, TState> ToState<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            Func<TState, TState> stateReducer)
            where TTransition : notnull
        {
            var dfaState = StateFactory<TTransition, TState>.SubState(current.AcceptedStates);

            return current.ToState(transition, dfaState, stateReducer);
        }

        public static AcceptedState<TState> ToAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            TState state)
            where TTransition : notnull
        {
            var acceptedStates = current.AcceptedStates;

            var dfaState = StateFactory<TTransition, TState>.Accepted(acceptedStates);
            var reducer = FConstant(state);

            current.ToState(transition, dfaState, reducer);

            var acceptedState = new AcceptedState<TState>(state);
            acceptedStates.Add(acceptedState, dfaState, reducer);

            return acceptedState;
        }

        public static AcceptedState<TState> ToAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            AcceptedState<TState> acceptedState)
            where TTransition : notnull
        {
            var acceptedStates = current.AcceptedStates;
            var stateReducerOpt = acceptedStates[acceptedState];

            return stateReducerOpt.Match(
                stateReducer =>
                {
                    var (state, reducer) = stateReducer;

                    current.ToState(transition, state, reducer);
                    return acceptedState;
                },
                () => acceptedState);
        }

        private static Func<TValue, TValue> FConstant<TValue>(TValue value) => _ => value;
    }
}