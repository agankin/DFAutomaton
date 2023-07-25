using Optional;

namespace DFAutomaton;

public delegate ReduceResult<TTransition, TState> Reduce<TTransition, TState>(AutomatonState<TTransition, TState> state)
where TTransition : notnull;

public readonly record struct ReduceResult<TTransition, TState>(
    TState State,
    Option<IState<TTransition, TState>> GoToState
)
where TTransition : notnull;