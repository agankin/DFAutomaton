namespace DFAutomaton
{
    public class AutomataRunState<TTransition>
    {
        private readonly Action<TTransition> _emitNext;

        public AutomataRunState(Action<TTransition> emitNext) => _emitNext = emitNext;

        public void EmitNext(TTransition transition) => _emitNext(transition);
    }
}