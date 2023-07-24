using Optional;

namespace DFAutomaton;

public delegate ReduceResult<TTransition, TState, TDFAState> Reduce<TTransition, TState, TDFAState>(AutomatonState<TTransition, TState> state)
where TTransition : notnull
where TDFAState : IState<TTransition, TState>;

public readonly record struct ReduceResult<TTransition, TState, TDFAState>(
    TState State,
    Option<TDFAState> GoToState
)
where TTransition : notnull
where TDFAState : IState<TTransition, TState>;