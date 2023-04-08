using DFAutomaton.Utils;
using Optional;

namespace DFAutomaton
{
    public class Automata<TTransition, TState> where TTransition : notnull
    {
        public Automata(IState<TTransition, TState> start) => Start = start;

        public IState<TTransition, TState> Start { get; }

        public Option<TState, AutomataError<TTransition, TState>> Run(TState startStateValue, IEnumerable<TTransition> transitions)
        {
            var initialAutomataState = new CurrentState(Start, startStateValue)
                .Some<CurrentState, AutomataError<TTransition, TState>>();

            var transitionsEnumerator = new TransitionsEnumerator<TTransition>(transitions);

            return transitionsEnumerator.ToEnumerable()
                .Aggregate(
                    initialAutomataState,
                    (automataState, transition) => Reduce(
                        transitionsEnumerator.QueueEmited,
                        automataState,
                        transition))
                .Map(automataState => automataState.StateValue);
        }

        private Option<CurrentState, AutomataError<TTransition, TState>> Reduce(
            Action<TTransition> emitNext,
            Option<CurrentState, AutomataError<TTransition, TState>> currentState,
            TTransition transition)
        {
            return currentState.FlatMap(Reduce(emitNext, transition));
        }

        private Func<CurrentState, Option<CurrentState, AutomataError<TTransition, TState>>> Reduce(
            Action<TTransition> emitNext,
            TTransition transition)
        {
            return automataState =>
            {
                var (state, stateValue) = automataState;

                return state[transition].Match(
                    Reduce(emitNext, stateValue),
                    () => Option.None<CurrentState, AutomataError<TTransition, TState>>(
                        GetErrorForTransitionNotFound(state, transition)));
            };
        }

        private Func<StateTransition<TTransition, TState>, Option<CurrentState, AutomataError<TTransition, TState>>> Reduce(
            Action<TTransition> emitNext,
            TState stateValue)
        {
            return automataNextState =>
            {
                var (nextState, reducer) = automataNextState;
                var runState = new AutomataRunState<TTransition, TState>(nextState, emitNext);
                var nextStateValue = reducer(runState, stateValue);

                var nextAutomataState = new CurrentState(nextState, nextStateValue);

                return Option.Some<CurrentState, AutomataError<TTransition, TState>>(nextAutomataState);
            };
        }

        private static AutomataError<TTransition, TState> GetErrorForTransitionNotFound(
            IState<TTransition, TState> state,
            TTransition transition)
        {
            var errorType = state.Type == StateType.Accepted
                ? AutomataErrorType.TransitionFromAccepted
                : AutomataErrorType.TransitionNotExists;

            return new AutomataError<TTransition, TState>(errorType, state, transition);
        }

        private readonly record struct CurrentState(
            IState<TTransition, TState> State,
            TState StateValue);
    }
}