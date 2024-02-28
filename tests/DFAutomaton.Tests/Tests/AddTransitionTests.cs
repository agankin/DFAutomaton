using NUnit.Framework;

namespace DFAutomaton.Tests;

[TestFixture(Description = "Tests for adding state transitions.")]
public partial class AddTransitionTests
{
    [Test(Description = "Tests adding transition to fixed state and constant value.")]
    public void Add_transition_to_fixed_state_with_constant_value()
    {
        var stateGraph = new StateGraph<StateActions, State>();
        var startState = stateGraph.StartState;

        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);

        var incState = startState.TransitsBy(StateActions.Inc).WithReducingTo(incValue).ToNew();

        incState.Is(StateType.SubState);
        startState[StateActions.Inc]
            .IsSome()
            .TransitsTo(incState)
            .Reduces(StateActions.Inc, value, incValue);
    }

    [Test(Description = "Tests adding transition to fixed state and value reducer.")]
    public void Add_transition_to_fixed_state_with_reducer()
    {
        var stateGraph = new StateGraph<StateActions, State>();
        var startState = stateGraph.StartState;

        var incState = startState.TransitsBy(StateActions.Inc).WithReducingBy(StateReducers.Inc).ToNew();
        
        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);

        incState.Is(StateType.SubState);
        startState[StateActions.Inc]
            .IsSome()
            .TransitsTo(incState)
            .Reduces(StateActions.Inc, value, incValue);
    }

    [Test(Description = "Tests linking existing state with transition to constant value.")]
    public void Link_fixed_state_with_constant_value_transition()
    {
        var stateGraph = new StateGraph<StateActions, State>();
        var startState = stateGraph.StartState;
        var incState = stateGraph.CreateState();
        
        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);
        
        var linkedState = startState.TransitsBy(StateActions.Inc).WithReducingTo(incValue).To(incState);
        Assert.AreEqual(incState, linkedState);
        linkedState.Is(StateType.SubState);
        
        startState[StateActions.Inc]
            .IsSome()
            .TransitsTo(incState)
            .Reduces(StateActions.Inc, value, incValue);
    }

    [Test(Description = "Tests linking existing state with transition by reducer.")]
    public void Link_fixed_state_with_value_transition_by_reducer()
    {
        var stateGraph = new StateGraph<StateActions, State>();
        var startState = stateGraph.StartState;
        var incState = stateGraph.CreateState();
        
        var linkedState = startState.TransitsBy(StateActions.Inc).WithReducingBy(StateReducers.Inc).To(incState);
        Assert.AreEqual(incState, linkedState);
        linkedState.Is(StateType.SubState);

        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);

        startState[StateActions.Inc]
            .IsSome()
            .TransitsTo(incState)
            .Reduces(StateActions.Inc, value, incValue);
    }
}