using Optional.Unsafe;

namespace DFAutomaton.Utils
{
    public static class StatesGraphFormatter<TTransition, TState> where TTransition : notnull
    {
        public static string Format(IState<TTransition, TState> startState)
        {
            var formattedStates = new HashSet<IState<TTransition, TState>>();
            var lines = GetStateLines(startState, formattedStates);

            return string.Join(Environment.NewLine, lines);
        }

        private static IEnumerable<string> GetStateLines(
            IState<TTransition, TState> state,
            ISet<IState<TTransition, TState>> formattedStates)
        {
            if (formattedStates.Contains(state))
                yield break;
            formattedStates.Add(state);

            yield return FormatState(state);
            
            var formattedTransitions = state.Transitions.Select(FormatTransition(state));
            foreach (var formattedTransition in formattedTransitions)
                yield return formattedTransition;

            yield return string.Empty;

            var nextStatesLines = state.Transitions
                .Select(transition => state[transition].ValueOrFailure().State)
                .SelectMany(nextState => GetStateLines(nextState, formattedStates));
            foreach (var nextStateLine in nextStatesLines)
                yield return nextStateLine;
        }

        private static Func<TTransition, string> FormatTransition(IState<TTransition, TState> fromState)
        {
            return transition =>
            {
                var (toState, reducer) = fromState[transition].ValueOrFailure();

                return FormatTransition(transition, toState);
            };
        }

        private static string FormatState(IState<TTransition, TState> state) =>
            state.Format();

        private static string FormatTransition(TTransition transition, IState<TTransition, TState> toState) =>
            $"    {transition} -> State {toState.Id}";
    }
}