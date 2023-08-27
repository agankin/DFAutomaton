using Optional;

namespace DFAutomaton;

/// <summary>
/// Automaton execution state.
/// </summary>
/// <typeparam name="TTransition">Transition value type.</typeparam>
/// <typeparam name="TState">State value type.</typeparam>
/// <param name="CurrentValue">Current automaton value.</param>
/// <param name="TransitingTo">Next state the automaton transiting to.</param>
/// <param name="Push">Function to dynamicly push next transition value.</param>
public record AutomatonState<TTransition, TState> where TTransition : notnull
{
    private Action<TTransition> _pushAction { get; init; }

    internal AutomatonState(
        TState currentValue,
        Option<IState<TTransition, TState>> transitingTo,
        Action<TTransition> pushAction)
    {
        CurrentValue = currentValue;
        TransitingTo = transitingTo;
        _pushAction = pushAction;
    }

    /// <summary>
    /// Current automaton value.
    /// </summary>
    public TState CurrentValue { get; private init; }
    
    /// <summary>
    /// Fixed next state the automaton transiting to or None for dynamic transition.
    /// </summary>
    public Option<IState<TTransition, TState>> TransitingTo { get; private init; }

    /// <summary>
    /// Pushes transition value to be retrieved next during automaton execution.
    /// </summary>
    /// <param name="transition">Transition value.</param>
    public void Push(TTransition transition) => _pushAction(transition);
}