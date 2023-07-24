namespace DFAutomaton;

public delegate TState ReduceValue<TTransition, TState>(AutomatonState<TTransition, TState> state) where TTransition : notnull;