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
            .TransitsBy(TO1).WithReducingTo(STATE1).ToSelf()
            .TransitsBy(TO2).WithReducingTo(STATE2).ToNew();
        var state3 = state1
            .TransitsBy(TO3).WithReducingTo(STATE3).ToNew();

        state2
            .TransitsBy(TOACCEPTED).WithReducingTo(ACCEPTED).ToAccepted();
        state3
            .TransitsBy(TOACCEPTED).WithReducingTo(ACCEPTED).ToAccepted();

        var automatonOrError = builder.Build(config => config.ValidateAnyReachesAccepted());
        automatonOrError.Value.IsSome();
    }

    [Test(Description = "Tests validation for no accepted state exists.")]
    public void Build_automaton_without_accepted()
    {
        var builder = AutomatonBuilder<Transitions, States>.Create();

        var state1 = builder.Start;
        var state2 = state1
            .TransitsBy(TO1).WithReducingTo(STATE1).ToSelf()
            .TransitsBy(TO2).WithReducingTo(STATE2).ToNew();
        var state3 = state1
            .TransitsBy(TO3).WithReducingTo(STATE3).ToNew();

        var automatonOrError = builder.Build(config => config.ValidateAnyReachesAccepted());
        automatonOrError.Value.IsError(ValidationError.NoAccepted);
    }

    [Test(Description = "Tests validation for accepted state not reachable from a state.")]
    public void Build_automaton_with_accepted_unreachable()
    {
        var builder = AutomatonBuilder<Transitions, States>.Create();

        var state1 = builder.Start;
        var state2 = state1
            .TransitsBy(TO1).WithReducingTo(STATE1).ToSelf()
            .TransitsBy(TO2).WithReducingTo(STATE2).ToNew();
        var state3 = state1
            .TransitsBy(TO3).WithReducingTo(STATE3).ToNew();

        state2
            .TransitsBy(TOACCEPTED).WithReducingTo(ACCEPTED).ToAccepted();

        var automatonOrError = builder.Build(config => config.ValidateAnyReachesAccepted());
        automatonOrError.Value.IsError(ValidationError.AcceptedIsUnreachable);
    }
}