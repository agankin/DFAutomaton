namespace DFAutomaton
{
    public static class StateGraphBuildingExtensions
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
            var nextState = StateFactory<TTransition, TState>.SubState();

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

        public static AcceptedStateHandle<TTransition, TState> ToNewAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            TState acceptedStateValue)
            where TTransition : notnull
        {
            var reducer = ConstantReducer<TTransition, TState>(acceptedStateValue);
            
            return current.ToNewAccepted(transition, reducer);
        }

        public static AcceptedStateHandle<TTransition, TState> ToNewAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            StateReducer<TTransition, TState> reducer)
            where TTransition : notnull
        {
            var acceptedState = StateFactory<TTransition, TState>.Accepted();
            current.LinkState(transition, acceptedState, reducer);

            return new AcceptedStateHandle<TTransition, TState>(acceptedState);
        }

        public static AcceptedStateHandle<TTransition, TState> LinkAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            AcceptedStateHandle<TTransition, TState> acceptedStateHandle,
            TState acceptedStateValue)
            where TTransition : notnull
        {
            var reducer = ConstantReducer<TTransition, TState>(acceptedStateValue);
            
            return current.LinkAccepted(transition, acceptedStateHandle, reducer);
        }

        public static AcceptedStateHandle<TTransition, TState> LinkAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            AcceptedStateHandle<TTransition, TState> acceptedStateHandle,
            StateReducer<TTransition, TState> reducer)
            where TTransition : notnull
        {
            var acceptedState = acceptedStateHandle.AcceptedState;
            current.LinkState(transition, acceptedState, reducer);
            
            return acceptedStateHandle;
        }

        private static StateReducer<TTransition, TState> ConstantReducer<TTransition, TState>(TState newState) =>
            (_, _) => newState;
    }
}