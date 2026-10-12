using NUnit.Framework;

namespace DFAutomaton.Tests;

using static Transitions;
using static States;

[TestFixture(Description = "Tests fixed transition building.")]
public class FixedTransitionBuildingTests
{
    [Test(Description = "Tests adding a fixed transition to a new state.")]
    public void Add_fixed_transition_to_new()
    {
        var state1 = new AutomatonBuilder<Transitions, States>().Start;
        var state2 = state1
            .ForInput(TO_STATE_2)
            .Reduce(STATE_2)
            .GoToNewState();
        var state3 = state1
            .ForInput(TO_STATE_3)
            .Reduce(Reducer.CreatePlain(TO_STATE_3, STATE_3))
            .GoToNewState();

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
        var automatonBuilder = new AutomatonBuilder<Transitions, States>();
        var state1 = automatonBuilder.Start;
        var existingState2 = automatonBuilder.CreateState();
        var existingState3 = automatonBuilder.CreateState();
        
        var state2 = state1
            .ForInput(TO_STATE_2)
            .Reduce(STATE_2)
            .GoTo(existingState2);
        var state3 = state1
            .ForInput(TO_STATE_3)
            .Reduce(Reducer.CreatePlain(TO_STATE_3, STATE_3))
            .GoTo(existingState3);

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
        var state1 = new AutomatonBuilder<Transitions, States>().Start;
        
        var incremenedState2 = state1
            .ForInput(TO_STATE_2)
            .Reduce(STATE_2)
            .GoToSelf();
        var incremenedState3 = state1
            .ForInput(TO_STATE_3)
            .Reduce(Reducer.CreatePlain(TO_STATE_3, STATE_3))
            .GoToSelf();

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
        var automatonBuilder = new AutomatonBuilder<Transitions, States>();
        var state1 = automatonBuilder.Start;
        var acceptedState2 = state1
            .ForInput(TO_STATE_2)
            .Reduce(STATE_2)
            .GoToAccepted();
        var acceptedState3 = state1
            .ForInput(TO_STATE_3)
            .Reduce(Reducer.CreatePlain(TO_STATE_3, STATE_3))
            .GoToAccepted();

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