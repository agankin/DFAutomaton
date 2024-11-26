namespace DFAutomaton;

/// <summary>
/// Represents a predicate testing a transition value can be used to perform a transition.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <param name="Name">A name of this predicate.</param>
/// <param name="Transition">A predicate function.</param>
public record CanTransit<TTransition>(
    string? Name,
    Predicate<TTransition> Predicate
)
where TTransition : notnull
{
    /// <summary>
    /// Tests the predicate against a transition value.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    /// <returns>The result of the predicate invocation.</returns>
    public bool Invoke(TTransition transition) => Predicate(transition);

    /// <inheritdoc/>
    public override string ToString() => string.IsNullOrEmpty(Name) ? "TRANSITION PREDICATE" : Name;
}