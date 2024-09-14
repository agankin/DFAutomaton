using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Contains a result of an automaton build.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public readonly struct BuildResult<TTransition, TState> where TTransition : notnull
{
    private readonly Result<Automaton<TTransition, TState>, ValidationError> _value;

    public BuildResult(Result<Automaton<TTransition, TState>, ValidationError> value) => _value = value;

    public TResult Match<TResult>(Func<Automaton<TTransition, TState>, TResult> matchAutomaton, Func<ValidationError, TResult> matchError)
    {
        return _value.Match(matchAutomaton, matchError);
    }

    public static implicit operator BuildResult<TTransition, TState>(Automaton<TTransition, TState> automaton) => new(automaton);

    public static implicit operator BuildResult<TTransition, TState>(ValidationError error) => new(error);

    public static implicit operator Result<Automaton<TTransition, TState>, ValidationError>(BuildResult<TTransition, TState> result) => result._value;
}