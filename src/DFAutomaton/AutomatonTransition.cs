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
    /// Automaton value before transition.
    /// </summary>
    public TState StateValueBefore { get; }

    /// <summary>
    /// Current transition.
    /// </summary>
    public TTransition Transition { get; }
    
    /// <summary>
    /// Fixed next state the automaton transiting to or None for dynamic transition.
    /// </summary>
    public Option<IState<TTransition, TState>> TransitingTo { get; private init; }

    /// <summary>
    /// Prepends transition value to be retrieved next during automaton execution.
    /// </summary>
    /// <param name="transition">Transition value.</param>
    public void Prepend(TTransition transition) => _prependNextTransition(transition);
}