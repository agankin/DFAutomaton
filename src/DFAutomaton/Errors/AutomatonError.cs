namespace DFAutomaton;

public class AutomatonError<TTransition, TState> where TTransition : notnull
{
    public AutomatonError(AutomatonErrorType type, IState<TTransition, TState> state, TTransition transition)
    {
        Type = type;
        State = state;
        Transition = transition;
    }

    public AutomatonErrorType Type { get; }

    public IState<TTransition, TState> State { get; }

    public TTransition Transition { get; }
}

public enum AutomatonErrorType
{
    TransitionNotExists = 1,

    TransitionFromAccepted
}