namespace DFAutomaton
{
    public readonly struct AutomataRunState<TTransition, TState> where TTransition : notnull
    {
        private readonly Action<TTransition> _emitNext;

        public AutomataRunState(IState<TTransition, TState> transitingTo, Action<TTransition> emitNext)
        {
            TransitingTo = transitingTo;
            _emitNext = emitNext;
        }

        public IState<TTransition, TState> TransitingTo { get; }

        public void EmitNext(TTransition transition) => _emitNext(transition);
    }
}