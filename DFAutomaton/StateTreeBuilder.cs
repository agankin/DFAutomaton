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
            var dfaState = StateFactory<TTransition, TState>.SubState(state, current.AcceptedStates);

            return current.ToState(transition, dfaState);
        }

        public static AcceptedState<TState> ToAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            TState state)
            where TTransition : notnull
        {
            var acceptedStates = current.AcceptedStates;

            var dfaState = StateFactory<TTransition, TState>.Accepted(state, acceptedStates);
            current.ToState(transition, dfaState);

            var acceptedState = new AcceptedState<TState>(state);
            acceptedStates.Add(acceptedState, dfaState);

            return acceptedState;
        }

        public static AcceptedState<TState> ToAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            AcceptedState<TState> acceptedState)
            where TTransition : notnull
        {
            var acceptedStates = current.AcceptedStates;
            var dfaStateOpt = acceptedStates[acceptedState];

            return dfaStateOpt.Match(
                dfaState =>
                {
                    current.ToState(transition, dfaState);
                    return acceptedState;
                },
                () => acceptedState);
        }
    }
}