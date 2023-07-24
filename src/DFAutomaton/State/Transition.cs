using Optional;

namespace DFAutomaton;

public record Transition<TTransition, TState, TDFAState>(
    TransitionType Type,
    Option<TDFAState> State,
    Reduce<TTransition, TState, TDFAState> Reduce
)
where TTransition : notnull
where TDFAState : IState<TTransition, TState>;

public enum TransitionType
{
    FixedState = 1,

    DynamicGoTo
}