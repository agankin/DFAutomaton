using Optional;

namespace DFAutomaton;

/// <summary>
/// Contains information about the current transition and provides methods for dynamic automaton control.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class AutomatonTransition<TTransition, TState> where TTransition : notnull
{
    private readonly StateGraph<TTransition, TState> _owningGraph;
    private readonly Action<TTransition> _yieldNext;

    internal AutomatonTransition(StateGraph<TTransition, TState> owningGraph, Action<TTransition> yieldNext)
    {
        _owningGraph = owningGraph;
        _yieldNext = yieldNext;
    }

    /// <summary>
    /// Contains Some with a next state the automaton is transiting to.
    /// </summary>
    /// <remarks>
    /// The next state is only known for fixed transitions.
    /// </remarks>
    public Option<ImmutableState<TTransition, TState>> TransitsTo { get; internal set; }

    /// <summary>
    /// A state the automaton must go to for a dynamic transition.
    /// </summary>
    internal Option<State<TTransition, TState>> DynamiclyGoToState { get; private set; }

    /// <summary>
    /// Yields the provided transition value into next automaton transitions enumeration.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    /// <remarks>
    /// The yielded value will be handled before initially provided transition values enumeration but after other previously yielded values.
    /// </remarks>
    public void YieldNext(TTransition transition) => _yieldNext(transition);

    /// <summary>
    /// Orders the automaton to go to a state with the provided id.
    /// </summary>
    /// <param name="stateId">The id of a state the automaton must dynamicly go to.</param>
    /// <remarks>
    /// This method calls are ignored for fixed transitions.
    /// </remarks>
    public void DynamiclyGoTo(Option<uint> stateId)
    {
        var state = stateId.Map(value => _owningGraph[value]);
        DynamiclyGoToState  = state;
    }
}