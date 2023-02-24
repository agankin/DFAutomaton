namespace DFAutomaton
{
    public class AutomataError
    {
        public AutomataError(AutomataErrorType type)
        {
            Type = type;
        }

        public AutomataErrorType Type { get; }
    }

    public enum AutomataErrorType
    {
        TransitionNotExists = 1
    }
}