using Optional;

namespace DFAutomaton;

public record AutomatonState<TTransition, TState>(
    TState CurrentValue,
    Option<IState<TTransition, TState>> TransitingTo,
    Action<TTransition> EmitNext
)
where TTransition : notnull;