namespace DFAutomaton.Utils
{
    internal class StateIdGenerator
    {
        public static StateIdDict<TTransition, TState> Generate<TTransition, TState>(
            State<TTransition, TState> startState)
            where TTransition : notnull
        {
            var stateIdDict = new StateIdDict<TTransition, TState>();
            AddStateId(stateIdDict, startState, currentId: 0);

            return stateIdDict;
        }

        private static int AddStateId<TTransition, TState>(
            StateIdDict<TTransition, TState> stateIdDict,
            State<TTransition, TState> state,
            int currentId)
            where TTransition : notnull
        {
            if (stateIdDict.ContainsState(state))
                return currentId;

            stateIdDict.GetValueOrNone(state).MatchNone(() => stateIdDict[state] = currentId++);

            return state.Transitions.Aggregate(
                currentId,
                (id, transition) => state[transition]
                    .Map(next => AddStateId(stateIdDict, next.State, id))
                    .ValueOr(id));
        }
    }
}