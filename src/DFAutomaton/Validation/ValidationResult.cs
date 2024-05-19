using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Contains a state graph validation result.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="Value">Some containing a start state of the hraph or None containing a validation error.</param>
public readonly record struct ValidationResult<TTransition, TState>(
    Result<State<TTransition, TState>, ValidationError> Value
)
where TTransition : notnull
{
    public static implicit operator ValidationResult<TTransition, TState>(State<TTransition, TState> state) => new(state);

    public static implicit operator ValidationResult<TTransition, TState>(ValidationError error) => new(error);

    public static implicit operator ValidationResult<TTransition, TState>(Result<State<TTransition, TState>, ValidationError> value)
    {
        return new(value);
    }
}