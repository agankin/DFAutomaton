using Optional;
using Optional.Collections;

namespace DFAutomaton;

/// <summary>
/// An automaton state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class State<TTransition, TState> : IState<TTransition, TState> where TTransition : notnull
{
    private readonly Dictionary<TTransition, State<TTransition, TState>.Transition> _transitionDict = new();

    internal State(StateType type, StateGraph<TTransition, TState> owningGraph)
    {
        Id = owningGraph.GenerateNextId();
        Type = type;
        OwningGraph = owningGraph;
    }

    /// <inheritdoc/>
    public long Id { get; }

    /// <inheritdoc/>
    public object? Tag { get; set; }

    /// <inheritdoc/>
    public StateType Type { get; }

    /// <inheritdoc/>
    public IReadOnlyCollection<TTransition> Transitions => new HashSet<TTransition>(_transitionDict.Keys);

    /// <inheritdoc/>
    public Option<Transition> this[TTransition transition] => _transitionDict.GetValueOrNone(transition);

    /// <summary>
    /// Returns an instance of transition configuration for building transition from this state.
    /// </summary>
    public TransitionBuilder<TTransition, TState> TransitsBy(TTransition transition) => new(this, transition);

    /// <inheritdoc/>
    Option<IState<TTransition, TState>.Transition> IState<TTransition, TState>.this[TTransition transition]
    {
        get
        {
            var stateTransition = _transitionDict.GetValueOrNone(transition);
            return stateTransition.Map(MapTransition);
        }
    }

    internal StateGraph<TTransition, TState> OwningGraph { get; }

    internal State<TTransition, TState> AddFixedTransition(TTransition transition, State<TTransition, TState> toState, Reduce<TTransition, TState> reduce)
    {
        ValidateLinkingNotAccepted();
        _transitionDict[transition] = new(TransitionKind.FixedState, toState.Some(), reduce);

        return toState;
    }
    
    internal void AddDynamicTransition(TTransition transition, Reduce<TTransition, TState> reduce)
    {
        ValidateLinkingNotAccepted();

        var noFixedGoToState = Option.None<State<TTransition, TState>>();
        _transitionDict[transition] = new(TransitionKind.DynamicGoTo, noFixedGoToState, reduce);
    }

    internal IReadOnlyDictionary<TTransition, Transition> GetTransitions() => _transitionDict;

    /// <inheritdoc/>
    public override string? ToString() => ((IState<TTransition, TState>)this).Format();

    private static IState<TTransition, TState>.Transition MapTransition(Transition transition)
    {
        var (kind, nextStateOption, reduce) = transition;
        var mappedNextStateOption = transition.State.Map<IState<TTransition, TState>>(_ => _);
        
        return new(kind, mappedNextStateOption, reduce);
    }

    private void ValidateLinkingNotAccepted()
    {
        if (Type == StateType.Accepted)
            throw new InvalidOperationException("Cannot link a state to the accepted state.");
    }

    /// <summary>
    /// Contains state transition data.
    /// </summary>
    /// <param name="Kind">The transition kind.</param>
    /// <param name="State">Some next state for fixed transitions or None for dynamic transitions.</param>
    /// <param name="Reduce">A function to reduce state value on transition.</param>
    public record Transition(
        TransitionKind Kind,
        Option<State<TTransition, TState>> State,
        Reduce<TTransition, TState> Reduce
    ) : Transition<TTransition, TState, State<TTransition, TState>>(Kind, State, Reduce);
}