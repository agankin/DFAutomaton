namespace DFAutomaton;

/// <summary>
/// Represents a predicate that determines whether an input value can trigger a transition.
/// </summary>
/// <typeparam name="TTransition">The input value type.</typeparam>
/// <param name="Name">The optional name of this predicate.</param>
/// <param name="Predicate">The predicate function to evaluate.</param>
public record InputPredicate<TTransition>(
    string? Name,
    Predicate<TTransition> Predicate
)
where TTransition : notnull
{
    /// <summary>
    /// Evaluates the predicate against the specified input value.
    /// </summary>
    /// <param name="transition">The input value to test.</param>
    /// <returns>
    /// <see langword="true"/> if the transition can be performed; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Invoke(TTransition transition) => Predicate(transition);

    /// <inheritdoc/>
    public override string ToString() => string.IsNullOrEmpty(Name) ? "TRANSITION PREDICATE" : Name;
}