using NUnit.Framework;

namespace DFAutomaton.Tests;

[TestFixture]
public class AutomatonStateGraphValidationTests
{
    [Test(Description = "Tests no error after valid automaton build.")]
    public void Build_valid_automaton()
    {
        var builder = AutomatonBuilder<Transition, State>.Create();
        var first = builder.Start;

        first
            .LinkFixedState(Transition.ToFirst, first, State.First)
            .ToNewFixedState(Transition.ToSecond, State.Second)
            .ToNewFixedState(Transition.ToThird, State.Third)
            .ToAccepted(Transition.ToAccepted, State.Accepted);

        var automatonOrError = builder.Build();
        automatonOrError.IsSome();
    }

    [Test(Description = "Tests validation for no accepted state exists.")]
    public void Build_automaton_without_accepted()
    {
        var builder = AutomatonBuilder<Transition, State>.Create();
        var first = builder.Start;

        first
            .LinkFixedState(Transition.ToFirst, first, State.First)
            .ToNewFixedState(Transition.ToSecond, State.Second)
            .ToNewFixedState(Transition.ToThird, State.Third);

        var automatonOrError = builder.Build();
        automatonOrError.IsError(ValidationError.NoAccepted);
    }

    [Test(Description = "Tests validation for accepted state not reachable from a state.")]
    public void Build_automaton_with_accepted_unreachable()
    {
        var builder = AutomatonBuilder<Transition, State>.Create();
        var first = builder.Start;

        var second = first
            .LinkFixedState(Transition.ToFirst, first, State.First)
            .ToNewFixedState(Transition.ToSecond, State.Second);

        second.ToNewFixedState(Transition.ToThird, State.Third);
        second.ToAccepted(Transition.ToAccepted, State.Accepted);

        var automatonOrError = builder.Build();
        automatonOrError.IsError(ValidationError.AcceptedIsUnreachable);
    }

    private enum State
    {
        First = 1,

        Second,

        Third,

        Accepted
    }

    private enum Transition
    {
        ToFirst = 1,

        ToSecond,

        ToThird,

        ToAccepted
    }
}