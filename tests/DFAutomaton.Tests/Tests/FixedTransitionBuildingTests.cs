using NUnit.Framework;

namespace DFAutomaton.Tests;

using static Transitions;
using static States;

[TestFixture(Description = "Tests fixed transition building.")]
public partial class FixedTransitionBuildingTests
{
    [Test(Description = "Tests adding a fixed transition to a new state.")]
    public void Add_fixed_transition_to_new()
    {
        var state1 = new StateGraph<Transitions, States>().StartState;
        var state2 = state1.TransitsBy(TO_STATE_2).WithReducingTo(STATE_2).ToNew();
        var state3 = state1.TransitsBy(TO_STATE_3).WithReducingBy(Reducer.Create(TO_STATE_3, STATE_3)).ToNew();

        state1[TO_STATE_2]
            .IsSome()
            .TransitsTo(state2)
            .Reduces(TO_STATE_2, STATE_1, STATE_2);
        state1[TO_STATE_3]
            .IsSome()
            .TransitsTo(state3)
            .Reduces(TO_STATE_3, STATE_1, STATE_3);
        state2.Has(StateType.SubState);
        state3.Has(StateType.SubState);
    }

    [Test(Description = "Tests adding a fixed transition to an existing state.")]
    public void Add_fixed_transition_to_existing()
    {
        var stateGraph = new StateGraph<Transitions, States>();
        var state1 = stateGraph.StartState;
        var existingState2 = stateGraph.CreateState();
        var existingState3 = stateGraph.CreateState();
        
        var state2 = state1.TransitsBy(TO_STATE_2).WithReducingTo(STATE_2).To(existingState2);
        var state3 = state1.TransitsBy(TO_STATE_3).WithReducingBy(Reducer.Create(TO_STATE_3, STATE_3)).To(existingState3);

        state1[TO_STATE_2]
            .IsSome()
            .TransitsTo(state2)
            .Reduces(TO_STATE_2, STATE_1, STATE_2);
        state1[TO_STATE_3]
            .IsSome()
            .TransitsTo(state3)
            .Reduces(TO_STATE_3, STATE_1, STATE_3);
        state2
            .ItIs(existingState2)
            .Has(StateType.SubState);
        state3
            .ItIs(existingState3)
            .Has(StateType.SubState);
    }

    [Test(Description = "Tests adding a fixed transition to self.")]
    public void Add_fixed_transition_to_self()
    {
        var state1 = new StateGraph<Transitions, States>().StartState;
        
        var incremenedState2 = state1.TransitsBy(TO_STATE_2).WithReducingTo(STATE_2).ToSelf();
        var incremenedState3 = state1.TransitsBy(TO_STATE_3).WithReducingBy(Reducer.Create(TO_STATE_3, STATE_3)).ToSelf();

        state1[TO_STATE_2]
            .IsSome()
            .TransitsTo(incremenedState2)
            .Reduces(TO_STATE_2, STATE_1, STATE_2);
        state1[TO_STATE_3]
            .IsSome()
            .TransitsTo(incremenedState3)
            .Reduces(TO_STATE_3, STATE_1, STATE_3);
        incremenedState2.ItIs(state1);
        incremenedState3.ItIs(state1);
    }

    [Test(Description = "Tests adding a fixed transition to the accepted state.")]
    public void Add_fixed_transition_to_accepted()
    {
        var stateGraph = new StateGraph<Transitions, States>();
        var state1 = stateGraph.StartState;
        var acceptedState2 = state1.TransitsBy(TO_STATE_2).WithReducingTo(STATE_2).ToAccepted();
        var acceptedState3 = state1.TransitsBy(TO_STATE_3).WithReducingBy(Reducer.Create(TO_STATE_3, STATE_3)).ToAccepted();

        state1[TO_STATE_2]
            .IsSome()
            .TransitsTo(StateType.Accepted)
            .Reduces(TO_STATE_2, STATE_1, STATE_2);
        state1[TO_STATE_3]
            .IsSome()
            .TransitsTo(StateType.Accepted)
            .Reduces(TO_STATE_3, STATE_1, STATE_3);
        
        acceptedState2.Id.ItIs(StateId.AcceptedStateId);
        acceptedState3.Id.ItIs(StateId.AcceptedStateId);
    }
}