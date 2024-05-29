using NUnit.Framework;

namespace DFAutomaton.Tests;

using static Transitions;
using static States;

[TestFixture]
public class AutomatonBuildValidationTests
{
    [Test(Description = "Tests no error after valid automaton build.")]
    public void Build_valid_automaton()
    {
        var builder = AutomatonBuilder<Transitions, States>.Create();
        
        var state1 = builder.Start;
        var state2 = state1
            .TransitsBy(TO_STATE_1).WithReducingTo(STATE_1).ToSelf()
            .TransitsBy(TO_STATE_2).WithReducingTo(STATE_2).ToNew();
        var state3 = state1
            .TransitsBy(TO_STATE_3).WithReducingTo(STATE_3).ToNew();

        state2
            .TransitsBy(TO_ACCEPTED).WithReducingTo(ACCEPTED).ToAccepted();
        state3
            .TransitsBy(TO_ACCEPTED).WithReducingTo(ACCEPTED).ToAccepted();

        var automatonOrError = builder
            .ValidateAnyCanReachAccepted()
            .Build();
        automatonOrError.Value.IsSome();
    }

    [Test(Description = "Tests validation for no accepted state exists.")]
    public void Build_automaton_without_accepted()
    {
        var builder = AutomatonBuilder<Transitions, States>.Create();

        var state1 = builder.Start;
        var state2 = state1
            .TransitsBy(TO_STATE_1).WithReducingTo(STATE_1).ToSelf()
            .TransitsBy(TO_STATE_2).WithReducingTo(STATE_2).ToNew();
        var state3 = state1
            .TransitsBy(TO_STATE_3).WithReducingTo(STATE_3).ToNew();

        var automatonOrError = builder
            .ValidateAnyCanReachAccepted()
            .Build();
        automatonOrError.Value.IsError(ValidationError.NoAccepted);
    }

    [Test(Description = "Tests validation for accepted state not reachable from a state.")]
    public void Build_automaton_with_accepted_unreachable()
    {
        var builder = AutomatonBuilder<Transitions, States>.Create();

        var state1 = builder.Start;
        var state2 = state1
            .TransitsBy(TO_STATE_1).WithReducingTo(STATE_1).ToSelf()
            .TransitsBy(TO_STATE_2).WithReducingTo(STATE_2).ToNew();
        var state3 = state1
            .TransitsBy(TO_STATE_3).WithReducingTo(STATE_3).ToNew();

        state2
            .TransitsBy(TO_ACCEPTED).WithReducingTo(ACCEPTED).ToAccepted();

        var automatonOrError = builder
            .ValidateAnyCanReachAccepted()
            .Build();
        automatonOrError.Value.IsError(ValidationError.AcceptedIsUnreachable);
    }
}