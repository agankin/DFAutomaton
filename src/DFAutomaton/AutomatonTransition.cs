using Optional;

namespace DFAutomaton;

/// <summary>
/// Contains automaton transition data.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class AutomatonTransition<TTransition, TState> where TTransition : notnull
{
    private readonly Action<TTransition> _yieldNext;

    internal AutomatonTransition(Action<TTransition> yieldNext) => _yieldNext = yieldNext;

    /// <summary>
    /// Contains Some next state the automaton transiting to on fixed transition or None on dynamic transition.
    /// </summary>
    public Option<IState<TTransition, TState>> TransitsTo { get; internal set; }

    /// <summary>
    /// A next state set inside a reducer dynamic transitions.
    /// </summary>
    internal Option<IState<TTransition, TState>> DynamiclyGoToState { get; private set; }

    /// <summary>
    /// Yields the provided transition value to be handled by the automaton before the initially provided transition sequence values
    /// but after the previously yielded values.
    /// </summary>
    /// <param name="transition">A transition value.</param>
    public void YieldNext(TTransition transition) => _yieldNext(transition);

    /// <summary>
    /// Orders the automaton to go to the provided state on the current transition. Calls ignored for fixed transitions.
    /// </summary>
    /// <param name="goToState">A state the automaton must dynamicly go to on dynamic transition.</param>
    public void DynamiclyGoTo(Option<IState<TTransition, TState>> state) => DynamiclyGoToState  = state;
}