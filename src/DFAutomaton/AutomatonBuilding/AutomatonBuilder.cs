using Optional;

namespace DFAutomaton;

public class AutomatonBuilder<TTransition, TState> where TTransition : notnull
{
    private AutomatonBuilder(State<TTransition, TState> startState) => StartState = startState;

    public State<TTransition, TState> StartState { get; }

    public static AutomatonBuilder<TTransition, TState> Create()
    {
        var startState = StateFactory<TTransition, TState>.Start();

        return new AutomatonBuilder<TTransition, TState>(startState);
    }

    public Option<Automaton<TTransition, TState>, AutomatonGraphError> Build()
    {
        var startOrError = StartState.BuildAutomatonGraph();

        return startOrError.Map(start => new Automaton<TTransition, TState>(start));
    }
}