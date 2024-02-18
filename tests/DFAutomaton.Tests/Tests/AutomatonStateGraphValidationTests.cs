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

        first.TransitsBy(Transition.ToFirst).WithReducing(State.First).ToSelf()
            .TransitsBy(Transition.ToSecond).WithReducing(State.Second).ToNew()
            .TransitsBy(Transition.ToThird).WithReducing(State.Third).ToNew()
            .TransitsBy(Transition.ToAccepted).WithReducing(State.Accepted).ToAccepted();

        var automatonOrError = builder.Build();
        automatonOrError.IsSome();
    }

    [Test(Description = "Tests validation for no accepted state exists.")]
    public void Build_automaton_without_accepted()
    {
        var builder = AutomatonBuilder<Transition, State>.Create();
        var first = builder.Start;

        first
            .TransitsBy(Transition.ToFirst).WithReducing(State.First).ToSelf()
            .TransitsBy(Transition.ToSecond).WithReducing(State.Second).ToNew()
            .TransitsBy(Transition.ToThird).WithReducing(State.Third).ToNew();

        var automatonOrError = builder.Build();
        automatonOrError.IsError(ValidationError.NoAccepted);
    }

    [Test(Description = "Tests validation for accepted state not reachable from a state.")]
    public void Build_automaton_with_accepted_unreachable()
    {
        var builder = AutomatonBuilder<Transition, State>.Create();
        var first = builder.Start;

        var second = first
            .TransitsBy(Transition.ToFirst).WithReducing(State.First).ToSelf()
            .TransitsBy(Transition.ToSecond).WithReducing(State.Second).ToNew();

        second.TransitsBy(Transition.ToThird).WithReducing(State.Third).ToNew();
        second.TransitsBy(Transition.ToAccepted).WithReducing(State.Accepted).ToAccepted();

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