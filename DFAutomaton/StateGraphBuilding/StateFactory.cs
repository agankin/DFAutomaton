namespace DFAutomaton
{
    internal static class StateFactory<TTransition, TState> where TTransition : notnull
    {
        public static State<TTransition, TState> Start() =>
            new State<TTransition, TState>(StateType.Start);

        public static State<TTransition, TState> SubState() =>
            new State<TTransition, TState>(StateType.SubState);

        public static State<TTransition, TState> Accepted() =>
            new State<TTransition, TState>(StateType.Accepted);
    }
}