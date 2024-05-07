using NUnit.Framework;

namespace DFAutomaton.Tests;

using static Transitions;
using static States;

[TestFixture]
public class AutomatonTests
{
    [Test(Description = "Automaton state transitions test scenario.")]
    public void Run_valid()
    {
        var automaton = TestAutomaton.Create();

        var transitions = new[]
        {
            TO_STATE_1,
            TO_STATE_2,
            TO_STATE_3,
            TO_STATE_4,
            TO_ACCEPTED,
        };

        var acceptedState = automaton.Run(STATE_1, transitions).IsSome();
        Assert.AreEqual(ACCEPTED, acceptedState);
    }

    [Test(Description = "Automaton error occuring on transition not found test scenario.")]
    public void Run_with_transition_not_found()
    {
        var automaton = TestAutomaton.Create();

        var transitions = new[]
        {
            TO_STATE_1,
            TO_STATE_2,
            TO_STATE_3,
            TO_STATE_5
        };

        automaton.Run(STATE_1, transitions)
            .IsError()
            .HasType(AutomatonErrorType.TransitionNotExists)
            .OccuredOn(TO_STATE_5)
            .WhenTransitioningFrom
                .IsSome()
                .Has(StateType.SubState);
    }

    [Test(Description = "Automaton error occuring on transition from accepted state test scenario.")]
    public void Run_with_transition_from_accepted()
    {
        var automaton = TestAutomaton.Create();

        var transitions = new[]
        {
            TO_STATE_1,
            TO_STATE_2,
            TO_STATE_3,
            TO_STATE_4,
            TO_ACCEPTED,
            TO_STATE_5
        };

        automaton.Run(STATE_1, transitions)
            .IsError()
            .HasType(AutomatonErrorType.TransitionFromAccepted)
            .OccuredOn(TO_STATE_5)
            .WhenTransitioningFrom
                .IsSome()
                .Has(StateType.Accepted);
    }
}