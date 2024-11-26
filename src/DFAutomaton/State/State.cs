using PureMonads;

namespace DFAutomaton;

/// <summary>
/// Represents an automaton state.
/// </summary>
/// <typeparam name="TTransition">The transition type.</typeparam>
/// <typeparam name="TState">The state type.</typeparam>
public readonly struct State<TTransition, TState> where TTransition : notnull
{    
    internal State(StateId id, StateGraph<TTransition, TState> owningGraph)
    {
        Id = id;
        OwningGraph = owningGraph;
    }

    /// <summary>
    /// Contains an identifier that is unique within the scope of the containing state graph.
    /// </summary>
    public StateId Id { get; }

    /// <summary>
    /// Contains a tag with additional information.
    /// </summary>
    /// <remarks>
    /// This information will be included in the text representation of the state returned from the <see cref="Format"/> method.
    /// </remarks>
    public object? Tag
    {
        get => OwningGraph.GetTag(Id);
        set => OwningGraph.SetTag(Id, value);
    }

    /// <summary>
    /// Contains the state type.
    /// </summary>
    public StateType Type => Id.GetStateType();

    /// <summary>
    /// Contains transitions to next states.
    /// </summary>
    public IReadOnlyCollection<StateTransition<TTransition, TState>> Transitions => OwningGraph.GetTransitions(Id);

    /// <summary>
    /// Returns a state transition by a transition value.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    /// <returns>An instance of <see cref="Option{Transition{TTransition, TState}}"/> containing a found state transition or None if no transition found.</returns>
    public Option<Transition<TTransition, TState>> this[TTransition transition] => OwningGraph.GetTransition(Id, transition);

    /// <summary>
    /// Returns an instance of <see cref="TransitionBuilder{TTransition, TState}"/> for building a new transition from this state.
    /// </summary>
    public TransitionBuilder<TTransition, TState> TransitsBy(TTransition transition) => new(this, transition);

    /// <summary>
    /// Returns an instance of <see cref="TransitionBuilder{TTransition, TState}"/> for building a new transition from this state.
    /// </summary>
    public TransitionBuilder<TTransition, TState> TransitsWhen(Predicate<TTransition> predicate)
    {
        var canTransit = new CanTransit<TTransition>(Name: null, predicate);
        return new(this, canTransit);
    }

    /// <summary>
    /// Returns an instance of <see cref="TransitionBuilder{TTransition, TState}"/> for building a new transition from this state.
    /// </summary>
    public TransitionBuilder<TTransition, TState> TransitsWhen(string predicateName, Predicate<TTransition> predicate)
    {
        var canTransit = new CanTransit<TTransition>(predicateName, predicate);
        return new(this, canTransit);
    }

    /// <summary>
    /// Returns an instance of <see cref="TransitionBuilder{TTransition, TState}"/> for building the fallback transition from this state.
    /// </summary>
    /// <remarks>
    /// Fallback transition is a per-state single dynamic transition that is invoked for all unknown transition values.
    /// </remarks>
    public FallbackTransitionBuilder<TTransition, TState> AllOtherTransits() => new(this);

    /// <summary>
    /// Returns a text representation of the state.
    /// </summary>
    public string Format() => string.IsNullOrEmpty(Tag?.ToString())
        ? $"State {Id}"
        : $"State {Id}: {Tag}";

    internal StateGraph<TTransition, TState> OwningGraph { get; }

    internal State<TTransition, TState> AddFixedTransition(
        StateId toStateId,
        Either<TTransition, CanTransit<TTransition>> byValueOrPredicate,
        Reduce<TTransition, TState> reducer)
    {
        ValidateLinkingNotAccepted();

        var toState = OwningGraph[toStateId];
        var transition = new Transition<TTransition, TState>(toState, reducer);
        OwningGraph.AddTransition(Id, byValueOrPredicate, transition);

        return OwningGraph[toStateId];
    }
    
    internal void AddDynamicTransition(
        Either<TTransition, CanTransit<TTransition>> byValueOrPredicate,
        Reduce<TTransition, TState> reducer)
    {
        ValidateLinkingNotAccepted();

        var noneGoToState = Option.None<State<TTransition, TState>>();
        var transition = new Transition<TTransition, TState>(noneGoToState, reducer);
        
        OwningGraph.AddTransition(Id, byValueOrPredicate, transition);
    }

    internal void AddFallbackTransition(Option<StateId> toStateId, Reduce<TTransition, TState> reducer)
    {
        ValidateLinkingNotAccepted();

        var owningGraph = OwningGraph;
        var toState = toStateId.Map(value => owningGraph[value]);
        var transition = new Transition<TTransition, TState>(toState, reducer);
        
        OwningGraph.AddFallbackTransition(Id, transition);
    }
    
    private void ValidateLinkingNotAccepted()
    {
        if (Type == StateType.Accepted)
            throw new InvalidOperationException("Cannot add transition from the accepted state.");
    }
}