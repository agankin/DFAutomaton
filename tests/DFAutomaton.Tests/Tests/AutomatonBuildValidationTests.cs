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
        var builder = new AutomatonBuilder<Transitions, States>();
        
        var state1 = builder.Start;
        var state2 = state1
            .ForInput(TO_STATE_1).Reduce(STATE_1).GoToSelf()
            .WhenInput(transition => transition == TO_STATE_2).Reduce(STATE_2).GoToNewState();
        var state3 = state1
            .ForInput(TO_STATE_3).Reduce(STATE_3).GoToNewState();
        var state4 = state3
            .WhenInput(transition => transition == TO_STATE_4).Reduce(STATE_4).GoToNewState();

        state2
            .ForInput(TO_ACCEPTED).Reduce(ACCEPTED).GoToAccepted();
        state4
            .ForInput(TO_ACCEPTED).Reduce(ACCEPTED).GoToAccepted();

        Result<Automaton<Transitions, States>, ValidationError> automatonOrError = builder
            .ValidateAnyCanReachAccepted()
            .Build();
        automatonOrError.IsSome();
    }

    [Test(Description = "Tests validation for no accepted state exists.")]
    public void Build_automaton_without_accepted()
    {
        var builder = new AutomatonBuilder<Transitions, States>();

        var state1 = builder.Start;
        var state2 = state1
            .ForInput(TO_STATE_1).Reduce(STATE_1).GoToSelf()
            .ForInput(TO_STATE_2).Reduce(STATE_2).GoToNewState();
        var state3 = state1
            .ForInput(TO_STATE_3).Reduce(STATE_3).GoToNewState();

        Result<Automaton<Transitions, States>, ValidationError> automatonOrError = builder
            .ValidateAnyCanReachAccepted()
            .Build();
        automatonOrError.IsError(ValidationError.NoAccepted);
    }

    [Test(Description = "Tests validation for accepted state not reachable from a state.")]
    public void Build_automaton_with_accepted_unreachable()
    {
        var builder = new AutomatonBuilder<Transitions, States>();

        var state1 = builder.Start;
        var state2 = state1
            .ForInput(TO_STATE_1).Reduce(STATE_1).GoToSelf()
            .ForInput(TO_STATE_2).Reduce(STATE_2).GoToNewState();
        var state3 = state1
            .ForInput(TO_STATE_3).Reduce(STATE_3).GoToNewState();

        state2
            .ForInput(TO_ACCEPTED).Reduce(ACCEPTED).GoToAccepted();

        Result<Automaton<Transitions, States>, ValidationError> automatonOrError = builder
            .ValidateAnyCanReachAccepted()
            .Build();
        automatonOrError.IsError(ValidationError.AcceptedIsUnreachable);
    }
}