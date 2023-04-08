namespace DFAutomaton
{
    public class AutomataError<TTransition, TState> where TTransition : notnull
    {
        public AutomataError(AutomataErrorType type, IState<TTransition, TState> state, TTransition transition)
        {
            Type = type;
            State = state;
            Transition = transition;
        }

        public AutomataErrorType Type { get; }

        public IState<TTransition, TState> State { get; }

        public TTransition Transition { get; }
    }

    public enum AutomataErrorType
    {
        TransitionNotExists = 1,

        TransitionFromAccepted
    }
}