using Optional;

namespace DFAutomaton;

/// <summary>
/// Contains a state graph validation result.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="Value">Some containing a start state of the hraph or None containing a validation error.</param>
public readonly record struct ValidationResult<TTransition, TState>(
    Option<State<TTransition, TState>, ValidationError> Value
)
where TTransition : notnull
{
    public static implicit operator ValidationResult<TTransition, TState>(State<TTransition, TState> state)
    {
        var value = state.Some<State<TTransition, TState>, ValidationError>();
        return new(value);
    }

    public static implicit operator ValidationResult<TTransition, TState>(ValidationError error)
    {
        var value = Option.None<State<TTransition, TState>, ValidationError>(error);
        return new(value);
    }

    public static implicit operator ValidationResult<TTransition, TState>(Option<State<TTransition, TState>, ValidationError> value)
    {
        return new(value);
    }
}