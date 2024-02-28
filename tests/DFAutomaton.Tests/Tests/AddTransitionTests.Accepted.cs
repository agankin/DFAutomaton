using NUnit.Framework;

namespace DFAutomaton.Tests;

public partial class AddTransitionTests
{
    [Test(Description = "Tests adding transition to accepted state and constant value.")]
    public void Add_transition_to_accepted_with_constant_value()
    {
        var stateGraph = new StateGraph<StateActions, State>();
        var startState = stateGraph.StartState;

        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);

        startState.TransitsBy(StateActions.Inc).WithReducingTo(incValue).ToAccepted();

        startState[StateActions.Inc]
            .IsSome()
            .Reduces(StateActions.Inc, value, incValue);
    }

    [Test(Description = "Tests adding transition to accepted state and value reducer.")]
    public void Add_transition_to_accepted_with_reducer()
    {
        var stateGraph = new StateGraph<StateActions, State>();
        var startState = stateGraph.StartState;
        
        startState.TransitsBy(StateActions.Inc).WithReducingBy(StateReducers.Inc).ToAccepted();
        
        var value = new State(StateValue.Initial);
        var incValue = new State(StateValue.Incremented);

        startState[StateActions.Inc]
            .IsSome()
            .Reduces(StateActions.Inc, value, incValue);
    }
}