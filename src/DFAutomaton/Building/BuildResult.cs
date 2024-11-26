using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Contains the result of an automaton build.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
public readonly struct BuildResult<TTransition, TState> where TTransition : notnull
{
    private readonly Result<Automaton<TTransition, TState>, ValidationError> _value;

    public BuildResult(Result<Automaton<TTransition, TState>, ValidationError> value) => _value = value;

    /// <summary>
    /// Matches the result by invoking a corresponding delegate.
    /// </summary>
    /// <typeparam name="TResult">The type of a value matching delegates return.</typeparam>
    /// <param name="onSuccess">A delegate invoked on matching build success.</param>
    /// <param name="onError">>A delegate invoked on matching build error.</param>
    /// <returns>A value returned from the matched delegate.</returns>
    public TResult Match<TResult>(Func<Automaton<TTransition, TState>, TResult> onSuccess, Func<ValidationError, TResult> onError)
    {
        return _value.Match(onSuccess, onError);
    }

    public static implicit operator BuildResult<TTransition, TState>(Automaton<TTransition, TState> automaton) => new(automaton);

    public static implicit operator BuildResult<TTransition, TState>(ValidationError error) => new(error);

    public static implicit operator Result<Automaton<TTransition, TState>, ValidationError>(BuildResult<TTransition, TState> result) => result._value;
}