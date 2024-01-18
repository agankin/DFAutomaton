using NUnit.Framework;

namespace DFAutomaton.Tests;

public partial class AddTransitionTests
{
    [Test(Description = "Tests adding transition to accepted state and constant value.")]
    public void Add_transition_to_accepted_with_constant_value()
    {
        var start = StateFactory<StateActions, State>.Start();

        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);

        var incState = start.ToAccepted(StateActions.Inc, incValue);

        incState.Is(StateType.Accepted);
        start[StateActions.Inc]
            .IsSome()
            .TransitsTo(TransitionKind.FixedState)
            .TransitsTo(incState)
            .Reduces(StateActions.Inc, value, incValue);
    }

    [Test(Description = "Tests adding transition to accepted state and value reducer.")]
    public void Add_transition_to_accepted_with_reducer()
    {
        var start = StateFactory<StateActions, State>.Start();
        var incState = start.ToNewFixedState(StateActions.Inc, StateReducers.Inc);
        
        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);

        incState.Is(StateType.SubState);
        start[StateActions.Inc]
            .IsSome()
            .TransitsTo(TransitionKind.FixedState)
            .TransitsTo(incState)
            .Reduces(StateActions.Inc, value, incValue);
    }
}