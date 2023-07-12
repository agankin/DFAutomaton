namespace DFAutomaton;

public readonly record struct Transition<TTransition, TState, TDFAState>(
    TDFAState NextState,
    StateReducer<TTransition, TState> Reducer
)
where TTransition : notnull
where TDFAState : IState<TTransition, TState>;