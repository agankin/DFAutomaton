namespace DFAutomaton
{
    internal static class StateFactory<TTransition, TState> where TTransition : notnull
    {
        public static State<TTransition, TState> Start()
        {
            var acceptedStates = new AcceptedStateDict<TTransition, TState>();
            return new State<TTransition, TState>(StateType.Start, acceptedStates);
        }

        public static State<TTransition, TState> SubState(AcceptedStateDict<TTransition, TState> acceptedStates) =>
            new State<TTransition, TState>(StateType.SubState, acceptedStates);

        public static State<TTransition, TState> Accepted(AcceptedStateDict<TTransition, TState> acceptedStates) =>
            new State<TTransition, TState>(StateType.Accepted, acceptedStates);
    }
}