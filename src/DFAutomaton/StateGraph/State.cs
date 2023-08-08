using Optional;
using Optional.Collections;

namespace DFAutomaton;

public class State<TTransition, TState> : IState<TTransition, TState> where TTransition : notnull
{
    private readonly Dictionary<TTransition, State<TTransition, TState>.Transition> _transitionDict = new();

    internal State(StateType type, Func<long> getNextId)
    {
        Id = getNextId();
        Type = type;
        GetNextId = getNextId;
    }

    public long Id { get; }

    public object? Tag { get; set; }

    public StateType Type { get; }

    public IReadOnlySet<TTransition> Transitions => new HashSet<TTransition>(_transitionDict.Keys);

    public Option<Transition> this[TTransition transition] => _transitionDict.GetValueOrNone(transition);

    Option<IState<TTransition, TState>.Transition> IState<TTransition, TState>.this[TTransition transition]
    {
        get
        {
            var stateTransition = _transitionDict.GetValueOrNone(transition);
            return stateTransition.Map<IState<TTransition, TState>.Transition>(MapTransition);
        }
    }

    internal Func<long> GetNextId { get; }

    public State<TTransition, TState> LinkFixedState(
        TTransition transition,
        State<TTransition, TState> nextState,
        ReduceValue<TTransition, TState> reduceValue)
    {
        ValidateLinkingNotAccepted();

        Reduce<TTransition, TState> reduce = automatonState =>
        {
            var reducedValue = reduceValue(automatonState);
            return new ReduceResult<TTransition, TState>(
                reducedValue,
                Option.None<IState<TTransition, TState>>());
        };
        _transitionDict[transition] = new(TransitionType.FixedState, nextState.Some(), reduce);

        return nextState;
    }
    
    public void LinkDynamic(TTransition transition, Reduce<TTransition, TState> reduce)
    {
        ValidateLinkingNotAccepted();
        _transitionDict[transition] = new(TransitionType.DynamicGoTo, Option.None<State<TTransition, TState>>(), reduce);
    }

    internal IReadOnlyDictionary<TTransition, Transition> GetTransitions() => _transitionDict;

    public override string? ToString() => ((IState<TTransition, TState>)this).Format();

    private static IState<TTransition, TState>.Transition MapTransition(Transition transition)
    {
        var (type, nextStateOption, reduce) = transition;
        var mappedNextStateOption = transition.State.Map<IState<TTransition, TState>>(_ => _);
        
        return new(type, mappedNextStateOption, reduce);
    }

    private void ValidateLinkingNotAccepted()
    {
        if (Type == StateType.Accepted)
            throw new InvalidOperationException("Cannot link a state to the accepted state.");
    }

    public record Transition(
        TransitionType Type,
        Option<State<TTransition, TState>> State,
        Reduce<TTransition, TState> Reduce
    ) : Transition<TTransition, TState, State<TTransition, TState>>(Type, State, Reduce);
}