using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Contains a state graph validation result.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
internal readonly struct ValidationResult<TTransition, TState> where TTransition : notnull
{
    private readonly Result<StateGraph<TTransition, TState>, ValidationError> _value;

    public ValidationResult(Result<StateGraph<TTransition, TState>, ValidationError> value) => _value = value;

    internal TResult Match<TResult>(Func<StateGraph<TTransition, TState>, TResult> matchStateGraph, Func<ValidationError, TResult> matchError)
    {
        return _value.Match(matchStateGraph, matchError);
    }

    public static implicit operator ValidationResult<TTransition, TState>(StateGraph<TTransition, TState> stateGraph) => new(stateGraph);

    public static implicit operator ValidationResult<TTransition, TState>(ValidationError error) => new(error);
}