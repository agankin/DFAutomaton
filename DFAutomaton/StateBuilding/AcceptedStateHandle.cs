namespace DFAutomaton
{
    public class AcceptedStateHandle<TTransition, TState>
        where TTransition : notnull
    {
        internal AcceptedStateHandle(State<TTransition, TState> acceptedState)
        {
            AcceptedState = acceptedState;
        }

        internal State<TTransition, TState> AcceptedState { get; }

        public object? Tag
        {
            get => AcceptedState.Tag;
            set => AcceptedState.Tag = value;
        }
    }
}