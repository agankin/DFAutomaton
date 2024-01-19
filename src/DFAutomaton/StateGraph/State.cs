using Optional;
using Optional.Collections;

namespace DFAutomaton;

/// <summary>
/// Automaton state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class State<TTransition, TState> : IState<TTransition, TState> where TTransition : notnull
{
    private readonly Dictionary<TTransition, State<TTransition, TState>.Transition> _transitionDict = new();

    internal State(StateType type, StateGraphContext<TTransition, TState> graphContext)
    {
        Id = graphContext.GenerateId();
        Type = type;
        GraphContext = graphContext;
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

    /// <inheritdoc/>
    Option<IState<TTransition, TState>.Transition> IState<TTransition, TState>.this[TTransition transition]
    {
        get
        {
            var stateTransition = _transitionDict.GetValueOrNone(transition);
            return stateTransition.Map(MapTransition);
        }
    }

    internal StateGraphContext<TTransition, TState> GraphContext { get; }

    /// <summary>
    /// Adds transition to fixed next state applying value reducer.
    /// </summary>
    /// <param name="transition">Transition value.</param>
    /// <param name="nextState">Next automaton state.</param>
    /// <param name="reduceValue">Automaton value reducer.</param>
    /// <returns>Next automaton state.</returns>
    public State<TTransition, TState> LinkFixedState(
        TTransition transition,
        State<TTransition, TState> nextState,
        ReduceValue<TTransition, TState> reduceValue)
    {
        Reduce<TTransition, TState> reduce = automatonTransition =>
        {
            var newStateValue = reduceValue(automatonTransition.Transition, automatonTransition.StateValueBefore);
            var noDynamicGoToState = Option.None<IState<TTransition, TState>>();
            
            return new ReductionResult<TTransition, TState>(newStateValue, noDynamicGoToState);
        };
        return LinkFixedState(transition, nextState, reduce);
    }

    /// <summary>
    /// Adds transition to fixed next state applying state transition reducer.
    /// </summary>
    /// <param name="transition">Transition value.</param>
    /// <param name="nextState">Next automaton state.</param>
    /// <param name="reduceTransition">Automaton transition reducer.</param>
    /// <returns>Next automaton state.</returns>
    public State<TTransition, TState> LinkFixedState(
        TTransition transition,
        State<TTransition, TState> nextState,
        ReduceTransition<TTransition, TState> reduceTransition)
    {
        Reduce<TTransition, TState> reduce = automatonTransition =>
        {
            var reducedValue = reduceTransition(automatonTransition);
            var noDynamicGoToState = Option.None<IState<TTransition, TState>>();
            
            return new ReductionResult<TTransition, TState>(reducedValue, noDynamicGoToState);
        };

        return LinkFixedState(transition, nextState, reduce);
    }
    
    /// <summary>
    /// Adds dynamic transition to next automaton state calculated by state reducer.
    /// </summary>
    /// <param name="transition">Transition value.</param>
    /// <param name="reduce">Automaton state reducer.</param>
    public void LinkDynamic(TTransition transition, Reduce<TTransition, TState> reduce)
    {
        ValidateLinkingNotAccepted();

        var noFixedGoToState = Option.None<State<TTransition, TState>>();
        _transitionDict[transition] = new(TransitionKind.DynamicGoTo, noFixedGoToState, reduce);
    }

    internal IReadOnlyDictionary<TTransition, Transition> GetTransitions() => _transitionDict;

    /// <inheritdoc/>
    public override string? ToString() => ((IState<TTransition, TState>)this).Format();

    private State<TTransition, TState> LinkFixedState(TTransition transition, State<TTransition, TState> nextState, Reduce<TTransition, TState> reduce)
    {
        ValidateLinkingNotAccepted();
        _transitionDict[transition] = new(TransitionKind.FixedState, nextState.Some(), reduce);

        return nextState;
    }

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
    /// State transition.
    /// </summary>
    /// <param name="Kind">Transition kind.</param>
    /// <param name="State">Some next state for fixed transition or None for dynamic transition.</param>
    /// <param name="Reduce">Automaton state reducer.</param>
    public record Transition(
        TransitionKind Kind,
        Option<State<TTransition, TState>> State,
        Reduce<TTransition, TState> Reduce
    ) : Transition<TTransition, TState, State<TTransition, TState>>(Kind, State, Reduce);
}