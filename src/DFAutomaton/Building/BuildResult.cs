using Optional;

namespace DFAutomaton;

/// <summary>
/// Contains the result of an automaton build.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="Value">Some containing a built automaton or None containing a validation error.</param>
public readonly record struct BuildResult<TTransition, TState>(
    Option<Automaton<TTransition, TState>, ValidationError> Value
)
where TTransition : notnull
{
    public static implicit operator BuildResult<TTransition, TState>(Automaton<TTransition, TState> automaton)
    {
        var value = automaton.Some<Automaton<TTransition, TState>, ValidationError>();
        return new(value);
    }

    public static implicit operator BuildResult<TTransition, TState>(ValidationError error)
    {
        var value = Option.None<Automaton<TTransition, TState>, ValidationError>(error);
        return new(value);
    }
}