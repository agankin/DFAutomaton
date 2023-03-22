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
            var control = new AutomataControl<TTransition>(transitionsEnumerator.QueueEmited);

            return transitionsEnumerator.ToEnumerable()
                .Aggregate(
                    initialAutomataState,
                    (automataState, transition) => Reduce(control, automataState, transition))
                .Map(automataState => automataState.StateValue);
        }

        private Option<CurrentState, AutomataError> Reduce(
            AutomataControl<TTransition> control,
            Option<CurrentState, AutomataError> currentState,
            TTransition transition)
        {
            return currentState.FlatMap(Reduce(control, transition));
        }

        private Func<CurrentState, Option<CurrentState, AutomataError>> Reduce(
            AutomataControl<TTransition> control,
            TTransition transition)
        {
            return automataState =>
            {
                var (state, stateValue) = automataState;

                return state[transition].Match(
                    Reduce(control, stateValue),
                    () => Option.None<CurrentState, AutomataError>(Error(AutomataErrorType.TransitionNotExists)));
            };
        }

        private Func<Next<TTransition, TState>, Option<CurrentState, AutomataError>> Reduce(
            AutomataControl<TTransition> control,
            TState stateValue)
        {
            return automataNextState =>
            {
                var (nextState, reducer) = automataNextState;
                var nextStateValue = reducer(control, stateValue);

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