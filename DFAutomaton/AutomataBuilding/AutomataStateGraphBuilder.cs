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
            var automataNextStates = new Dictionary<TTransition, StateTransition<TTransition, TState>>();
            var automataState = buildedStates[state] = new AutomataState<TTransition, TState>(
                state.Id,
                state.Tag,
                type,
                automataNextStates);

            state.GetTransitions()
                .ToAutomataTransitions(buildedStates)
                .CopyTo(automataNextStates);

            return automataState;
        }

        private static IReadOnlyDictionary<TTransition, StateTransition<TTransition, TState>> ToAutomataTransitions<TTransition, TState>(
            this IReadOnlyDictionary<TTransition, Transition<TTransition, TState>> nextStates,
            IDictionary<State<TTransition, TState>, AutomataState<TTransition, TState>> buildedStates)
            where TTransition : notnull
        {
            return nextStates.ToDictionary(
                nextState => nextState.Key,
                nextState => nextState.Value.ToAutomataTransition(buildedStates));
        }

        private static StateTransition<TTransition, TState> ToAutomataTransition<TTransition, TState>(
            this Transition<TTransition, TState> nextState,
            IDictionary<State<TTransition, TState>, AutomataState<TTransition, TState>> buildedStates)
            where TTransition : notnull
        {
            var (state, reducer) = nextState;
            var automataState = buildedStates.GetValueOrNone(state)
                .Match(
                    automataState => automataState,
                    () => state.ToAutomataState(buildedStates));

            return new StateTransition<TTransition, TState>(automataState, reducer);
        }

        private static void CopyTo<TTransition, TState>(
            this IReadOnlyDictionary<TTransition, StateTransition<TTransition, TState>> sourceDict,
            IDictionary<TTransition, StateTransition<TTransition, TState>> destDict)
            where TTransition : notnull
        {
            foreach (var keyValue in sourceDict)
                destDict[keyValue.Key] = keyValue.Value;
        }
    }
}