using PureMonads;

namespace DFAutomaton;

internal readonly record struct TransitionEntry<TTransition, TState>(
    Option<StateId> ToStateId,
    Reduce<TTransition, TState> Reducer
)
where TTransition : notnull;