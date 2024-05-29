using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Contains a state graph validation result.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="Value">Some containing a start state of the hraph or None containing a validation error.</param>
internal readonly record struct ValidationResult<TTransition, TState>(
    Result<StateGraph<TTransition, TState>, ValidationError> Value
)
where TTransition : notnull
{
    public static implicit operator ValidationResult<TTransition, TState>(StateGraph<TTransition, TState> stateGraph) => new(stateGraph);

    public static implicit operator ValidationResult<TTransition, TState>(ValidationError error) => new(error);

    public static implicit operator ValidationResult<TTransition, TState>(Result<StateGraph<TTransition, TState>, ValidationError> value)
    {
        return new(value);
    }
}