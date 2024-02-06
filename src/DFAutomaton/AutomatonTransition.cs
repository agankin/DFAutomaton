using Optional;

namespace DFAutomaton;

/// <summary>
/// Automaton transition.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
public class AutomatonTransition<TTransition, TState> where TTransition : notnull
{
    private Action<TTransition> _prependNextTransition { get; init; }

    internal AutomatonTransition(
        TState stateValueBefore,
        TTransition transition,
        Option<IState<TTransition, TState>> transitingTo,
        Action<TTransition> prependNextTransition)
    {
        StateValueBefore = stateValueBefore;
        Transition = transition;
        TransitingTo = transitingTo;
        _prependNextTransition = prependNextTransition;
    }

    /// <summary>
    /// Contains a state value before transition.
    /// </summary>
    public TState StateValueBefore { get; }

    /// <summary>
    /// Contains the current transition value.
    /// </summary>
    public TTransition Transition { get; }
    
    /// <summary>
    /// Contains Some next state the automaton transiting to for a fixed transition or None for dynamic transition.
    /// </summary>
    public Option<IState<TTransition, TState>> TransitingTo { get; private init; }

    /// <summary>
    /// Pushes the provided transition value ahead of transition sequence to be returned next.
    /// </summary>
    /// <param name="transition">Transition value.</param>
    public void Prepend(TTransition transition) => _prependNextTransition(transition);
}