using Optional;

namespace DFAutomaton
{
    public class Automata<TTransition, TState>
        where TTransition : notnull
    {
        public Automata(AutomataState<TTransition, TState> start) => Start = start;

        public AutomataState<TTransition, TState> Start { get; }

        public Option<TState, AutomataError> Run(TState startStateValue, IEnumerable<TTransition> transitions)
        {
            var initialAutomataState = Option.Some<CurrentState, AutomataError>(
                new CurrentState(Start, startStateValue));

            return transitions.Aggregate(initialAutomataState, Reduce)
                .Map(automataState => automataState.StateValue);
        }

        private Option<CurrentState, AutomataError> Reduce(
            Option<CurrentState, AutomataError> currentState,
            TTransition transition) =>
            currentState.FlatMap(Reduce(transition));

        private Func<CurrentState, Option<CurrentState, AutomataError>> Reduce(TTransition transition) =>
            automataState =>
            {
                var (dfaState, state) = automataState;

                return dfaState[transition].Match(
                    Reduce(state),
                    () => Option.None<CurrentState, AutomataError>(Error(AutomataErrorType.TransitionNotExists)));
            };

        private Func<AutomataNextState<TTransition, TState>, Option<CurrentState, AutomataError>> Reduce(TState stateValue) =>
            automataNextState =>
            {
                var (nextState, reducer) = automataNextState;
                var nextStateValue = reducer(stateValue);

                var nextAutomataState = new CurrentState(nextState, nextStateValue);

                return Option.Some<CurrentState, AutomataError>(nextAutomataState);
            };

        private static AutomataError Error(AutomataErrorType type) => new AutomataError(type);

        private readonly record struct CurrentState(
            AutomataState<TTransition, TState> State,
            TState StateValue);
    }
}