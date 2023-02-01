using Optional;

namespace DFAutomaton
{
    internal static class StateFactory<TTransition, TState>
        where TTransition : notnull
    {
        public static State<TTransition, TState> Start()
        {
            var acceptedStates = new AcceptedStateDict<TTransition, TState>();
            var startStateValue = Option.None<TState>();

            return new State<TTransition, TState>(StateType.Start, startStateValue, acceptedStates);
        }

        public static State<TTransition, TState> SubState(
            TState value,
            AcceptedStateDict<TTransition, TState> acceptedStates)
        {
            var subStateValue = Option.Some(value);

            return new State<TTransition, TState>(StateType.SubState, subStateValue, acceptedStates);
        }

        public static State<TTransition, TState> Accepted(
            TState state,
            AcceptedStateDict<TTransition, TState> acceptedStates)
        {
            var acceptedStateValue = Option.Some(state);

            return new State<TTransition, TState>(StateType.SubState, acceptedStateValue, acceptedStates);
        }
    }
}