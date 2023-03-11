namespace DFAutomaton
{
    public class AutomataControl<TTransition>
    {
        private readonly Action<TTransition> _emitNext;

        public AutomataControl(Action<TTransition> emitNext) => _emitNext = emitNext;

        public void EmitNext(TTransition transition) => _emitNext(transition);
    }
}