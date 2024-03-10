using Optional;

namespace DFAutomaton;

/// <summary>
/// An automaton state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public readonly struct State<TTransition, TState> where TTransition : notnull
{
    private readonly StateId _id;
    
    internal State(StateId id, StateGraph<TTransition, TState> owningGraph)
    {
        _id = id;
        OwningGraph = owningGraph;
    }

    /// <summary>
    /// Contains an identifier that is unique within the scope of the containing state graph.
    /// </summary>
    public uint Id => _id;

    /// <summary>
    /// Contains a tag with additional information.
    /// </summary>
    /// <remarks>
    /// This information will be included in the text representation of the state returned from <see cref="Format"/> method.
    /// </remarks>
    public object? Tag
    {
        get => OwningGraph.GetTag(_id);
        set => OwningGraph.SetTag(_id, value);
    }

    /// <summary>
    /// Contains the state type.
    /// </summary>
    public StateType Type => _id.GetStateType();

    /// <summary>
    /// Contains transitions to next states.
    /// </summary>
    public IReadOnlyCollection<TTransition> Transitions => OwningGraph.GetTransitions(_id);

    /// <summary>
    /// Returns a state transition by a transition value.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    /// <returns>Some state transition for the provided transition value or None if the transition doesn't exist.</returns>
    public Option<Transition<TTransition, TState>> this[TTransition transition] => OwningGraph.GetStateTransition(_id, transition);

    /// <summary>
    /// Returns an instance of transition builder for building a transition from this state.
    /// </summary>
    public TransitionBuilder<TTransition, TState> TransitsBy(TTransition transition) => new(this, transition);

    /// <summary>
    /// Returns an instance of transition builder for building a fallback transition from this state.
    /// </summary>
    /// <remarks>
    /// Fallback transition is a dynamic transition that is invoked for all unknown transition values.
    /// </remarks>
    public FallbackTransitionBuilder<TTransition, TState> AllOtherTransits() => new(this);

    /// <summary>
    /// Returns a text representation of the state.
    /// </summary>
    public string Format() => string.IsNullOrEmpty(Tag?.ToString())
        ? $"State {Id}"
        : $"State {Id}: {Tag}";

    internal StateGraph<TTransition, TState> OwningGraph { get; }

    internal State<TTransition, TState> AddFixedTransition(uint toStateId, TTransition transition, Reduce<TTransition, TState> reducer)
    {
        ValidateLinkingNotAccepted();

        var toState = OwningGraph[toStateId];
        var stateTransition = new Transition<TTransition, TState>(toState.Some(), reducer);
        OwningGraph.AddStateTransition(_id, transition, stateTransition);

        return OwningGraph[toStateId];
    }
    
    internal void AddDynamicTransition(TTransition transition, Reduce<TTransition, TState> reducer)
    {
        ValidateLinkingNotAccepted();

        var noneGoToState = Option.None<State<TTransition, TState>>();
        var stateTransition = new Transition<TTransition, TState>(noneGoToState, reducer);
        
        OwningGraph.AddStateTransition(_id, transition, stateTransition);
    }

    internal void AddFallbackTransition(Reduce<TTransition, TState> reducer)
    {
        ValidateLinkingNotAccepted();

        var noneGoToState = Option.None<State<TTransition, TState>>();
        var stateTransition = new Transition<TTransition, TState>(noneGoToState, reducer);
        
        OwningGraph.AddFallbackTransition(_id, stateTransition);
    }
    
    private void ValidateLinkingNotAccepted()
    {
        if (Type == StateType.Accepted)
            throw new InvalidOperationException("Cannot add transition from the accepted state.");
    }
}