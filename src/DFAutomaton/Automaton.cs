﻿using DFAutomaton.Utils;
using Optional;

namespace DFAutomaton
{
    public class Automaton<TTransition, TState> where TTransition : notnull
    {
        public Automaton(IState<TTransition, TState> start) => Start = start;

        public IState<TTransition, TState> Start { get; }

        public Option<TState, AutomatonError<TTransition, TState>> Run(TState startStateValue, IEnumerable<TTransition> transitions)
        {
            var initialAutomatonState = new CurrentState(Start, startStateValue)
                .Some<CurrentState, AutomatonError<TTransition, TState>>();

            var transitionsEnumerator = new TransitionsEnumerator<TTransition>(transitions);

            return transitionsEnumerator.ToEnumerable()
                .Aggregate(
                    initialAutomatonState,
                    (automatonState, transition) => Reduce(
                        transitionsEnumerator.QueueEmited,
                        automatonState,
                        transition))
                .Map(automatonState => automatonState.StateValue);
        }

        private Option<CurrentState, AutomatonError<TTransition, TState>> Reduce(
            Action<TTransition> emitNext,
            Option<CurrentState, AutomatonError<TTransition, TState>> currentState,
            TTransition transition)
        {
            return currentState.FlatMap(Reduce(emitNext, transition));
        }

        private Func<CurrentState, Option<CurrentState, AutomatonError<TTransition, TState>>> Reduce(
            Action<TTransition> emitNext,
            TTransition transition)
        {
            return automatonState =>
            {
                var (state, stateValue) = automatonState;

                return state[transition].Match(
                    Reduce(emitNext, stateValue),
                    () => Option.None<CurrentState, AutomatonError<TTransition, TState>>(
                        GetErrorForTransitionNotFound(state, transition)));
            };
        }

        private Func<StateTransition<TTransition, TState>, Option<CurrentState, AutomatonError<TTransition, TState>>> Reduce(
            Action<TTransition> emitNext,
            TState stateValue)
        {
            return automatonNextState =>
            {
                var (nextState, reducer) = automatonNextState;
                var runState = new AutomatonRunState<TTransition, TState>(nextState, emitNext);
                var nextStateValue = reducer(runState, stateValue);

                var nextAutomatonState = new CurrentState(nextState, nextStateValue);

                return Option.Some<CurrentState, AutomatonError<TTransition, TState>>(nextAutomatonState);
            };
        }

        private static AutomatonError<TTransition, TState> GetErrorForTransitionNotFound(
            IState<TTransition, TState> state,
            TTransition transition)
        {
            var errorType = state.Type == StateType.Accepted
                ? AutomatonErrorType.TransitionFromAccepted
                : AutomatonErrorType.TransitionNotExists;

            return new AutomatonError<TTransition, TState>(errorType, state, transition);
        }

        private readonly record struct CurrentState(
            IState<TTransition, TState> State,
            TState StateValue);
    }
}