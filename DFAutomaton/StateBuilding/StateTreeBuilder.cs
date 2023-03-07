﻿namespace DFAutomaton
{
    public static class StateTreeBuilder
    {
        public static State<TTransition, TState> ToNewState<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            TState nextStateValue)
            where TTransition : notnull
        {
            return current.ToNewState(transition, ConstantReducer(nextStateValue));
        }

        public static State<TTransition, TState> ToNewState<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            Func<TState, TState> reducer)
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
            var reducer = ConstantReducer(nextStateValue);
            return current.LinkState(transition, nextState, reducer);
        }

        public static AcceptedStateHandle<TState> ToNewAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            TState acceptedStateValue)
            where TTransition : notnull
        {
            return current.ToNewAccepted(transition, ConstantReducer(acceptedStateValue));
        }

        public static AcceptedStateHandle<TState> ToNewAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            Func<TState, TState> reducer)
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
            var reducer = ConstantReducer(acceptedStateValue);
            return current.LinkAccepted(transition, acceptedStateHandle, reducer);
        }

        public static AcceptedStateHandle<TState> LinkAccepted<TTransition, TState>(
            this State<TTransition, TState> current,
            TTransition transition,
            AcceptedStateHandle<TState> acceptedStateHandle,
            Func<TState, TState> reducer)
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

        private static Func<TValue, TValue> ConstantReducer<TValue>(TValue value) => _ => value;
    }
}