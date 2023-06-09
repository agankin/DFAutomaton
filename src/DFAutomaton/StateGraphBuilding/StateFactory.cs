using DFAutomaton.Utils;

namespace DFAutomaton
{
    internal static class StateFactory<TTransition, TState> where TTransition : notnull
    {
        public static State<TTransition, TState> Start()
        {
            var getNextId = StateIdGenerator.CreateNew();

            return new State<TTransition, TState>(StateType.Start, getNextId);
        }

        public static State<TTransition, TState> SubState(Func<long> getNextId) =>
            new State<TTransition, TState>(StateType.SubState, getNextId);

        public static State<TTransition, TState> Accepted(Func<long> getNextId) =>
            new State<TTransition, TState>(StateType.Accepted, getNextId);
    }
}