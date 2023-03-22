using Optional.Unsafe;

namespace DFAutomaton.Utils
{
    public static class StatesGraphFormatter<TTransition, TState> where TTransition : notnull
    {
        public static string Format(IState<TTransition, TState> startState)
        {
            var stateIdDict = StateIdGenerator.Generate(startState);
            var formattedStates = new HashSet<IState<TTransition, TState>>();

            var lines = GetStateLines(startState, stateIdDict, formattedStates);

            return string.Join(Environment.NewLine, lines);
        }

        private static IEnumerable<string> GetStateLines(
            IState<TTransition, TState> state,
            StateIdDict<TTransition, TState> stateIdDict,
            ISet<IState<TTransition, TState>> formattedStates)
        {
            if (formattedStates.Contains(state))
                yield break;

            formattedStates.Add(state);

            var stateId = stateIdDict[state];
            yield return $"State {stateId}: {state.Tag}";
            
            var formattedTransitions = state.Transitions.Select(FormatTransition(state, stateIdDict));
            foreach (var formattedTransition in formattedTransitions)
                yield return formattedTransition;

            yield return string.Empty;

            var nextStatesLines = state.Transitions
                .Select(transition => state[transition].ValueOrFailure().State)
                .SelectMany(nextState => GetStateLines(nextState, stateIdDict, formattedStates));
            foreach (var nextStateLine in nextStatesLines)
                yield return nextStateLine;
        }

        private static Func<TTransition, string> FormatTransition(
            IState<TTransition, TState> fromState,
            StateIdDict<TTransition, TState> stateIdDict)
        {
            return transition =>
            {
                var (nextState, reducer) = fromState[transition].ValueOrFailure();
                var nextStateId = stateIdDict[nextState];

                return $"    {transition} -> State {nextStateId}";
            };
        }
    }
}