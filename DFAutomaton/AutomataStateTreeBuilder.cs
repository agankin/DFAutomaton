using Optional.Collections;

namespace DFAutomaton
{
    public static class AutomataStateTreeBuilder
    {
        public static AutomataState<TTransition, TState> BuildAutomataTree<TTransition, TState>(
            this State<TTransition, TState> start)
            where TTransition : notnull
        {
            var buildedStates = new Dictionary<State<TTransition, TState>, AutomataState<TTransition, TState>>();

            return start.ToAutomataState(buildedStates);
        }

        private static AutomataState<TTransition, TState> ToAutomataState<TTransition, TState>(
            this State<TTransition, TState> state,
            IDictionary<State<TTransition, TState>, AutomataState<TTransition, TState>> buildedStates)
            where TTransition : notnull
        {
            var type = state.Type;
            var automataNextStates = state.GetNextStates().ToAutomataNextStates(buildedStates);

            return new AutomataState<TTransition, TState>(type, automataNextStates);
        }

        private static IReadOnlyDictionary<TTransition, AutomataNextState<TTransition, TState>> ToAutomataNextStates<TTransition, TState>(
            this IReadOnlyDictionary<TTransition, NextState<TTransition, TState>> nextStates,
            IDictionary<State<TTransition, TState>, AutomataState<TTransition, TState>> buildedStates)
            where TTransition : notnull
        {
            return nextStates.ToDictionary(
                nextState => nextState.Key,
                nextState => nextState.Value.ToAutomataNextState(buildedStates));
        }

        private static AutomataNextState<TTransition, TState> ToAutomataNextState<TTransition, TState>(
            this NextState<TTransition, TState> nextState,
            IDictionary<State<TTransition, TState>, AutomataState<TTransition, TState>> buildedStates)
            where TTransition : notnull
        {
            var (state, reducer) = nextState;
            var automataState = buildedStates.GetValueOrNone(state)
                .Match(
                    automataState => automataState,
                    () => buildedStates[state] = state.ToAutomataState(buildedStates));

            return new AutomataNextState<TTransition, TState>(automataState, reducer);
        }
    }
}