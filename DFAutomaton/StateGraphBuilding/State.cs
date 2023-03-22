﻿using Optional;
using Optional.Collections;

namespace DFAutomaton
{
    public class State<TTransition, TState> : IState<TTransition, TState>
        where TTransition : notnull
    {
        private readonly Dictionary<TTransition, NextState<TTransition, TState>> _nextStates = new();

        internal State(StateType type, Func<long> getNextId)
        {
            Id = getNextId();
            Type = type;
            GetNextId = getNextId;
        }

        public long Id { get; }

        public object? Tag { get; set; }

        public StateType Type { get; }

        public IReadOnlySet<TTransition> Transitions => new HashSet<TTransition>(_nextStates.Keys);

        public Option<NextState<TTransition, TState>> this[TTransition transition] =>
            _nextStates.GetValueOrNone(transition);

        Option<Next<TTransition, TState>> IState<TTransition, TState>.this[TTransition transition] =>
            _nextStates.GetValueOrNone(transition)
                .Map(next => new Next<TTransition, TState>(next.State, next.Reducer));

        internal Func<long> GetNextId { get; }

        public State<TTransition, TState> LinkState(
            TTransition transition,
            State<TTransition, TState> nextState,
            StateReducer<TTransition, TState> reducer)
        {
            if (Type == StateType.Accepted)
                throw new InvalidOperationException("Cannot link a state to the accepted state.");

            var (state, _) = _nextStates[transition] = new NextState<TTransition, TState>(nextState, reducer);

            return state;
        }

        internal IReadOnlyDictionary<TTransition, NextState<TTransition, TState>> GetNextStates() => _nextStates;

        public override string? ToString() => this.Format();
    }
}