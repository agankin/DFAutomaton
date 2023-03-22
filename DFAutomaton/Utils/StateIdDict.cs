using Optional;
using Optional.Collections;

namespace DFAutomaton.Utils
{
    internal class StateIdDict<TTransition, TState> where TTransition : notnull
    {
        private readonly Dictionary<IState<TTransition, TState>, int> _stateIdDict = new();

        public int this[IState<TTransition, TState> state]
        {
            get => _stateIdDict[state];
            set => _stateIdDict[state] = value;
        }

        public bool ContainsState(IState<TTransition, TState> state) => _stateIdDict.ContainsKey(state);

        public Option<int> GetValueOrNone(IState<TTransition, TState> state) => _stateIdDict.GetValueOrNone(state);
    }
}