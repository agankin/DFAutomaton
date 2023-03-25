﻿using Optional;
using Optional.Collections;

namespace DFAutomaton
{
    public class State<TTransition, TState> : IState<TTransition, TState>
        where TTransition : notnull
    {
        private readonly Dictionary<TTransition, Transition<TTransition, TState>> _transitions = new();

        internal State(StateType type, Func<long> getNextId)
        {
            Id = getNextId();
            Type = type;
            GetNextId = getNextId;
        }

        public long Id { get; }

        public object? Tag { get; set; }

        public StateType Type { get; }

        public IReadOnlySet<TTransition> Transitions => new HashSet<TTransition>(_transitions.Keys);

        public Option<Transition<TTransition, TState>> this[TTransition transition] =>
            _transitions.GetValueOrNone(transition);

        Option<StateTransition<TTransition, TState>> IState<TTransition, TState>.this[TTransition transition] =>
            _transitions.GetValueOrNone(transition)
                .Map(next => new StateTransition<TTransition, TState>(next.NextState, next.Reducer));

        internal Func<long> GetNextId { get; }

        public State<TTransition, TState> LinkState(
            TTransition transition,
            State<TTransition, TState> nextState,
            StateReducer<TTransition, TState> reducer)
        {
            if (Type == StateType.Accepted)
                throw new InvalidOperationException("Cannot link a state to the accepted state.");

            var (state, _) = _transitions[transition] = new Transition<TTransition, TState>(nextState, reducer);

            return state;
        }

        internal IReadOnlyDictionary<TTransition, Transition<TTransition, TState>> GetTransitions() => _transitions;

        public override string? ToString() => this.Format();
    }
}