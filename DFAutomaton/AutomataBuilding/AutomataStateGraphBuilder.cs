using Optional.Collections;

namespace DFAutomaton
{
    internal static class AutomataStateGraphBuilder
    {
        public static IState<TTransition, TState> BuildAutomataGraph<TTransition, TState>(
            this State<TTransition, TState> start)
            where TTransition : notnull
        {
            var buildedStates = new Dictionary<State<TTransition, TState>, AutomataState<TTransition, TState>>();

            return start.ToAutomataState(buildedStates);
        }

        private static IState<TTransition, TState> ToAutomataState<TTransition, TState>(
            this State<TTransition, TState> state,
            IDictionary<State<TTransition, TState>, AutomataState<TTransition, TState>> buildedStates)
            where TTransition : notnull
        {
            var type = state.Type;
            var automataNextStates = new Dictionary<TTransition, Next<TTransition, TState>>();
            var automataState = buildedStates[state] =
                new AutomataState<TTransition, TState>(state.Tag, type, automataNextStates);

            state.GetNextStates()
                .ToAutomataNextStates(buildedStates)
                .CopyTo(automataNextStates);

            return automataState;
        }

        private static IReadOnlyDictionary<TTransition, Next<TTransition, TState>> ToAutomataNextStates<TTransition, TState>(
            this IReadOnlyDictionary<TTransition, NextState<TTransition, TState>> nextStates,
            IDictionary<State<TTransition, TState>, AutomataState<TTransition, TState>> buildedStates)
            where TTransition : notnull
        {
            return nextStates.ToDictionary(
                nextState => nextState.Key,
                nextState => nextState.Value.ToAutomataNextState(buildedStates));
        }

        private static Next<TTransition, TState> ToAutomataNextState<TTransition, TState>(
            this NextState<TTransition, TState> nextState,
            IDictionary<State<TTransition, TState>, AutomataState<TTransition, TState>> buildedStates)
            where TTransition : notnull
        {
            var (state, reducer) = nextState;
            var automataState = buildedStates.GetValueOrNone(state)
                .Match(
                    automataState => automataState,
                    () => state.ToAutomataState(buildedStates));

            return new Next<TTransition, TState>(automataState, reducer);
        }

        private static void CopyTo<TTransition, TState>(
            this IReadOnlyDictionary<TTransition, Next<TTransition, TState>> sourceDict,
            IDictionary<TTransition, Next<TTransition, TState>> destDict)
            where TTransition : notnull
        {
            foreach (var keyValue in sourceDict)
                destDict[keyValue.Key] = keyValue.Value;
        }
    }
}