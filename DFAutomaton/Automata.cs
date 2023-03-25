using DFAutomaton.Utils;
using Optional;

namespace DFAutomaton
{
    public class Automata<TTransition, TState> where TTransition : notnull
    {
        public Automata(IState<TTransition, TState> start) => Start = start;

        public IState<TTransition, TState> Start { get; }

        public Option<TState, AutomataError> Run(TState startStateValue, IEnumerable<TTransition> transitions)
        {
            var initialAutomataState = Option.Some<CurrentState, AutomataError>(
                new CurrentState(Start, startStateValue));

            var transitionsEnumerator = new TransitionsEnumerator<TTransition>(transitions);
            var runState = new AutomataRunState<TTransition>(transitionsEnumerator.QueueEmited);

            return transitionsEnumerator.ToEnumerable()
                .Aggregate(
                    initialAutomataState,
                    (automataState, transition) => Reduce(runState, automataState, transition))
                .Map(automataState => automataState.StateValue);
        }

        private Option<CurrentState, AutomataError> Reduce(
            AutomataRunState<TTransition> runState,
            Option<CurrentState, AutomataError> currentState,
            TTransition transition)
        {
            return currentState.FlatMap(Reduce(runState, transition));
        }

        private Func<CurrentState, Option<CurrentState, AutomataError>> Reduce(
            AutomataRunState<TTransition> runState,
            TTransition transition)
        {
            return automataState =>
            {
                var (state, stateValue) = automataState;

                return state[transition].Match(
                    Reduce(runState, stateValue),
                    () => Option.None<CurrentState, AutomataError>(Error(AutomataErrorType.TransitionNotExists)));
            };
        }

        private Func<StateTransition<TTransition, TState>, Option<CurrentState, AutomataError>> Reduce(
            AutomataRunState<TTransition> runState,
            TState stateValue)
        {
            return automataNextState =>
            {
                var (nextState, reducer) = automataNextState;
                var nextStateValue = reducer(runState, stateValue);

                var nextAutomataState = new CurrentState(nextState, nextStateValue);

                return Option.Some<CurrentState, AutomataError>(nextAutomataState);
            };
        }

        private static AutomataError Error(AutomataErrorType type) => new AutomataError(type);

        private readonly record struct CurrentState(
            IState<TTransition, TState> State,
            TState StateValue);
    }
}