﻿using Optional;
using Optional.Collections;

namespace DFAutomaton
{
    public class State<TTransition, TState>
        where TTransition : notnull
    {
        private readonly Dictionary<TTransition, NextState<TTransition, TState>> _nextStates = new();

        internal State(StateType type, AcceptedStateDict<TTransition, TState> acceptedStates)
        {
            Type = type;
            AcceptedStates = acceptedStates;
        }

        public StateType Type { get; }

        public IReadOnlySet<TTransition> Transitions => new HashSet<TTransition>(_nextStates.Keys);

        internal AcceptedStateDict<TTransition, TState> AcceptedStates { get; }

        public Option<NextState<TTransition, TState>> this[TTransition transition] =>
            _nextStates.GetValueOrNone(transition);

        internal State<TTransition, TState> ToState(
            TTransition transition,
            State<TTransition, TState> dfaState,
            Func<TState, TState> reducer)
        {
            var (state, _) = _nextStates[transition] = new NextState<TTransition, TState>(dfaState, reducer);

            return state;
        }

        internal IReadOnlyDictionary<TTransition, NextState<TTransition, TState>> GetNextStates() =>
            _nextStates;
    }
}