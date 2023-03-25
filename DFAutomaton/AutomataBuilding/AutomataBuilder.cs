namespace DFAutomaton
{
    public class AutomataBuilder<TTransition, TState> where TTransition : notnull
    {
        private AutomataBuilder(State<TTransition, TState> startState) => StartState = startState;

        public State<TTransition, TState> StartState { get; }

        public static AutomataBuilder<TTransition, TState> Create()
        {
            var startState = StateFactory<TTransition, TState>.Start();

            return new AutomataBuilder<TTransition, TState>(startState);
        }

        public Automata<TTransition, TState> Build()
        {
            var automataStartState = StartState.BuildAutomataGraph();

            return new Automata<TTransition, TState>(automataStartState);
        }
    }
}