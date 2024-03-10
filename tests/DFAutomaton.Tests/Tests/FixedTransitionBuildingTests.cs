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
        var state2 = state1.TransitsBy(TO2).WithReducingTo(STATE2).ToNew();
        var state3 = state1.TransitsBy(TO3).WithReducingBy(Reducer.Create(TO3, STATE3)).ToNew();

        state1[TO2]
            .IsSome()
            .TransitsTo(state2)
            .Reduces(TO2, STATE1, STATE2);
        state1[TO3]
            .IsSome()
            .TransitsTo(state3)
            .Reduces(TO3, STATE1, STATE3);
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
        
        var state2 = state1.TransitsBy(TO2).WithReducingTo(STATE2).To(existingState2);
        var state3 = state1.TransitsBy(TO3).WithReducingBy(Reducer.Create(TO3, STATE3)).To(existingState3);

        state1[TO2]
            .IsSome()
            .TransitsTo(state2)
            .Reduces(TO2, STATE1, STATE2);
        state1[TO3]
            .IsSome()
            .TransitsTo(state3)
            .Reduces(TO3, STATE1, STATE3);
        state2
            .Is(existingState2)
            .Has(StateType.SubState);
        state3
            .Is(existingState3)
            .Has(StateType.SubState);
    }

    [Test(Description = "Tests adding a fixed transition to self.")]
    public void Add_fixed_transition_to_self()
    {
        var state1 = new StateGraph<Transitions, States>().StartState;
        
        var incremenedState2 = state1.TransitsBy(TO2).WithReducingTo(STATE2).ToSelf();
        var incremenedState3 = state1.TransitsBy(TO3).WithReducingBy(Reducer.Create(TO3, STATE3)).ToSelf();

        state1[TO2]
            .IsSome()
            .TransitsTo(incremenedState2)
            .Reduces(TO2, STATE1, STATE2);
        state1[TO3]
            .IsSome()
            .TransitsTo(incremenedState3)
            .Reduces(TO3, STATE1, STATE3);
        incremenedState2.Is(state1);
        incremenedState3.Is(state1);
    }

    [Test(Description = "Tests adding a fixed transition to the accepted state.")]
    public void Add_fixed_transition_to_accepted()
    {
        var stateGraph = new StateGraph<Transitions, States>();
        var state1 = stateGraph.StartState;
        var acceptedState2 = state1.TransitsBy(TO2).WithReducingTo(STATE2).ToAccepted();
        var acceptedState3 = state1.TransitsBy(TO3).WithReducingBy(Reducer.Create(TO3, STATE3)).ToAccepted();

        state1[TO2]
            .IsSome()
            .TransitsTo(StateType.Accepted)
            .Reduces(TO2, STATE1, STATE2);
        state1[TO3]
            .IsSome()
            .TransitsTo(StateType.Accepted)
            .Reduces(TO3, STATE1, STATE3);
        
        Assert.AreEqual(acceptedState2.Id, StateId.AcceptedStateId);
        Assert.AreEqual(acceptedState3.Id, StateId.AcceptedStateId);
    }
}