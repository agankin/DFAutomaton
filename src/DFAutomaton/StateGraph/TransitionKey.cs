namespace DFAutomaton;

internal readonly record struct TransitionKey<TTransition>(
    StateId FromStateId,
    TTransition Transition
);