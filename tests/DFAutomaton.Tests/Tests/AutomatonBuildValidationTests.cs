using NUnit.Framework;
using PureMonads;

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
            .TransitsWhen(transition => transition == TO_STATE_2).WithReducingTo(STATE_2).ToNew();
        var state3 = state1
            .TransitsBy(TO_STATE_3).WithReducingTo(STATE_3).ToNew();
        var state4 = state3
            .TransitsWhen(transition => transition == TO_STATE_4).WithReducingTo(STATE_4).ToNew();

        state2
            .TransitsBy(TO_ACCEPTED).WithReducingTo(ACCEPTED).ToAccepted();
        state4
            .TransitsBy(TO_ACCEPTED).WithReducingTo(ACCEPTED).ToAccepted();

        Result<Automaton<Transitions, States>, ValidationError> automatonOrError = builder
            .ValidateAnyCanReachAccepted()
            .Build();
        automatonOrError.IsSome();
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

        Result<Automaton<Transitions, States>, ValidationError> automatonOrError = builder
            .ValidateAnyCanReachAccepted()
            .Build();
        automatonOrError.IsError(ValidationError.NoAccepted);
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

        Result<Automaton<Transitions, States>, ValidationError> automatonOrError = builder
            .ValidateAnyCanReachAccepted()
            .Build();
        automatonOrError.IsError(ValidationError.AcceptedIsUnreachable);
    }
}