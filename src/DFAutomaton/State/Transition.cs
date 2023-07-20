using Optional;

namespace DFAutomaton;

public record Transition<TTransition, TState, TDFAState>(
    Option<TDFAState> State,
    GoToState<TTransition, TState, TDFAState> GoToState,
    Reduce<TState> Reduce
)
where TTransition : notnull
where TDFAState : IState<TTransition, TState>;

public delegate TDFAState GoToState<TTransition, TState, out TDFAState>(TState state)
    where TTransition : notnull
    where TDFAState : IState<TTransition, TState>;

public delegate TState Reduce<TState>(TState state);