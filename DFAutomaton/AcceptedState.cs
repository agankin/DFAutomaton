namespace DFAutomaton
{
    public class AcceptedState<TState>
    {
        public AcceptedState(TState state) => State = state;

        public TState State { get; }
    }
}