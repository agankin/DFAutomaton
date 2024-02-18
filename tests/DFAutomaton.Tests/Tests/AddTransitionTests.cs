using NUnit.Framework;

namespace DFAutomaton.Tests;

[TestFixture(Description = "Tests for adding state transitions.")]
public partial class AddTransitionTests
{
    [Test(Description = "Tests adding transition to fixed state and constant value.")]
    public void Add_transition_to_fixed_state_with_constant_value()
    {
        var start = StateFactory<StateActions, State>.Start();

        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);

        var incState = start.TransitsBy(StateActions.Inc).WithReducing(incValue).ToNew();

        incState.Is(StateType.SubState);
        start[StateActions.Inc]
            .IsSome()
            .TransitsTo(TransitionKind.FixedState)
            .TransitsTo(incState)
            .Reduces(StateActions.Inc, value, incValue);
    }

    [Test(Description = "Tests adding transition to fixed state and value reducer.")]
    public void Add_transition_to_fixed_state_with_reducer()
    {
        var start = StateFactory<StateActions, State>.Start();
        var incState = start.TransitsBy(StateActions.Inc).WithReducing(StateReducers.Inc).ToNew();
        
        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);

        incState.Is(StateType.SubState);
        start[StateActions.Inc]
            .IsSome()
            .TransitsTo(TransitionKind.FixedState)
            .TransitsTo(incState)
            .Reduces(StateActions.Inc, value, incValue);
    }

    [Test(Description = "Tests linking existing state with transition to constant value.")]
    public void Link_fixed_state_with_constant_value_transition()
    {
        var start = StateFactory<StateActions, State>.Start();
        var incState = StateFactory<StateActions, State>.SubState(start.OwningGraph);
        
        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);
        
        var linkedState = start.TransitsBy(StateActions.Inc).WithReducing(incValue).To(incState);
        Assert.AreEqual(incState, linkedState);
        linkedState.Is(StateType.SubState);
        
        start[StateActions.Inc]
            .IsSome()
            .TransitsTo(TransitionKind.FixedState)
            .TransitsTo(incState)
            .Reduces(StateActions.Inc, value, incValue);
    }

    [Test(Description = "Tests linking existing state with transition by reducer.")]
    public void Link_fixed_state_with_value_transition_by_reducer()
    {
        var start = StateFactory<StateActions, State>.Start();
        var incState = StateFactory<StateActions, State>.SubState(start.OwningGraph);
        
        var linkedState = start.TransitsBy(StateActions.Inc).WithReducing(StateReducers.Inc).To(incState);
        Assert.AreEqual(incState, linkedState);
        linkedState.Is(StateType.SubState);

        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);

        start[StateActions.Inc]
            .IsSome()
            .TransitsTo(TransitionKind.FixedState)
            .TransitsTo(incState)
            .Reduces(StateActions.Inc, value, incValue);
    }
}