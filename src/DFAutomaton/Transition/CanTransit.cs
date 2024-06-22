namespace DFAutomaton;

/// <summary>
/// Contains a predicate returning can transition be performed for a transition value..
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <param name="Name">A name of this predicate.</param>
/// <param name="Transition">A predicate.</param>
public record CanTransit<TTransition>(
    string? Name,
    Predicate<TTransition> Predicate
)
where TTransition : notnull
{
    /// <summary>
    /// Invokes the predicate for a transition value.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    /// <returns>The result of the predicate invocation.</returns>
    public bool Invoke(TTransition transition) => Predicate(transition);

    /// <inheritdoc/>
    public override string ToString() => string.IsNullOrEmpty(Name) ? "TRANSITION PREDICATE" : Name;
}