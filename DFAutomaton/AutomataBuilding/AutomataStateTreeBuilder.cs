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
            var automataNextStates = new Dictionary<TTransition, AutomataNextState<TTransition, TState>>();
            var automataState = buildedStates[state] =
                new AutomataState<TTransition, TState>(type, automataNextStates);

            state.GetNextStates()
                .ToAutomataNextStates(buildedStates)
                .CopyTo(automataNextStates);

            return automataState;
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
                    () => state.ToAutomataState(buildedStates));

            return new AutomataNextState<TTransition, TState>(automataState, reducer);
        }

        private static void CopyTo<TTransition, TState>(
            this IReadOnlyDictionary<TTransition, AutomataNextState<TTransition, TState>> sourceDict,
            IDictionary<TTransition, AutomataNextState<TTransition, TState>> destDict)
            where TTransition : notnull
        {
            foreach (var keyValue in sourceDict)
                destDict[keyValue.Key] = keyValue.Value;
        }
    }
}