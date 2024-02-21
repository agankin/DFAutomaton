namespace DFAutomaton.Tests;

public partial class AddTransitionTests
{
    private record State(StateValue Value);

    private enum StateValue
    {
        Initial,

        Incremented
    }

    private enum StateActions
    {
        Inc = 1
    }

    private static class StateReducers
    {
        public static State Inc(State state, StateActions _) => state with { Value = StateValue.Incremented };
    }
}