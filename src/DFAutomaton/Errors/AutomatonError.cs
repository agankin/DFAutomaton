namespace DFAutomaton;

public record AutomatonError<TTransition, TState>(
    AutomatonErrorType Type,
    IState<TTransition, TState> State,
    TTransition Transition
)
where TTransition : notnull;

public enum AutomatonErrorType
{
    TransitionNotExists = 1,

    TransitionFromAccepted
}