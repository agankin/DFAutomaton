namespace DFAutomaton
{
    public static class StateTreeBuilder
    {
        public static State<TTransition, TState> ToNewState<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            TState nextStateValue)
            where TTransition : notnull
        {
            var reducer = ConstantReducer<TTransition, TState>(nextStateValue);

            return current.ToNewState(transition, reducer);
        }

        public static State<TTransition, TState> ToNewState<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            StateReducer<TTransition, TState> reducer)
            where TTransition : notnull
        {
            var nextState = StateFactory<TTransition, TState>.SubState(current.AcceptedStates);

            return current.LinkState(transition, nextState, reducer);
        }

        public static State<TTransition, TState> LinkState<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            State<TTransition, TState> nextState,
            TState nextStateValue)
            where TTransition : notnull
        {
            var reducer = ConstantReducer<TTransition, TState>(nextStateValue);
            
            return current.LinkState(transition, nextState, reducer);
        }

        public static AcceptedStateHandle<TState> ToNewAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            TState acceptedStateValue)
            where TTransition : notnull
        {
            var reducer = ConstantReducer<TTransition, TState>(acceptedStateValue);
            
            return current.ToNewAccepted(transition, reducer);
        }

        public static AcceptedStateHandle<TState> ToNewAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            StateReducer<TTransition, TState> reducer)
            where TTransition : notnull
        {
            var acceptedStates = current.AcceptedStates;
            var acceptedState = StateFactory<TTransition, TState>.Accepted(acceptedStates);

            current.LinkState(transition, acceptedState, reducer);

            var acceptedStateHandle = new AcceptedStateHandle<TState>();
            acceptedStates.Add(acceptedStateHandle, acceptedState);

            return acceptedStateHandle;
        }

        public static AcceptedStateHandle<TState> LinkAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            AcceptedStateHandle<TState> acceptedStateHandle,
            TState acceptedStateValue)
            where TTransition : notnull
        {
            var reducer = ConstantReducer<TTransition, TState>(acceptedStateValue);
            
            return current.LinkAccepted(transition, acceptedStateHandle, reducer);
        }

        public static AcceptedStateHandle<TState> LinkAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            AcceptedStateHandle<TState> acceptedStateHandle,
            StateReducer<TTransition, TState> reducer)
            where TTransition : notnull
        {
            var acceptedStates = current.AcceptedStates;
            var acceptedStateOption = acceptedStates[acceptedStateHandle];

            return acceptedStateOption.Match(
                acceptedState =>
                {
                    current.LinkState(transition, acceptedState, reducer);
                    return acceptedStateHandle;
                },
                () => acceptedStateHandle);
        }

        private static StateReducer<TTransition, TState> ConstantReducer<TTransition, TState>(TState newState) =>
            (_, _) => newState;
    }
}