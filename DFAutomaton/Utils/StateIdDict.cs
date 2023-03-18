using Optional;
using Optional.Collections;

namespace DFAutomaton.Utils
{
    internal class StateIdDict<TTransition, TState> where TTransition : notnull
    {
        private readonly Dictionary<State<TTransition, TState>, int> _stateIdDict = new();

        public int this[State<TTransition, TState> state]
        {
            get => _stateIdDict[state];
            set => _stateIdDict[state] = value;
        }

        public bool ContainsState(State<TTransition, TState> state) => _stateIdDict.ContainsKey(state);

        public Option<int> GetValueOrNone(State<TTransition, TState> state) => _stateIdDict.GetValueOrNone(state);
    }
}