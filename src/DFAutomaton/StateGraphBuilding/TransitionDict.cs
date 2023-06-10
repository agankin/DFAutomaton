namespace DFAutomaton;

internal class TransitionDict<TTransition, TState> : Dictionary<TTransition, Transition<TTransition, TState>>, ITransitionDict<TTransition, TState>
    where TTransition : notnull 
{
}