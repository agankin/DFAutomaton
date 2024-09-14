namespace DFAutomaton.Tests;

using static Transitions;
using static States;

internal static class TestAutomaton
{
    public static Automaton<Transitions, States> Create()
    {
        var builder = AutomatonBuilder<Transitions, States>.Create();
        
        var state1 = builder.Start;
        var state3 = builder.CreateState();
        var state4 = builder.CreateState();
        var state5 = builder.CreateState();
        var state6 = builder.CreateState();

        var state2 = state1
            .TransitsBy(TO_STATE_1)
                .WithReducingTo(STATE_1)
                .ToSelf()
            .TransitsWhen(transition => transition == TO_STATE_2)
                .WithReducingBy((_, _) => STATE_2)
                .ToNew();

        state2.TransitsBy(TO_STATE_3)
            .Dynamicly()
            .WithReducing((_, _) => new ReductionResult<Transitions, States>(STATE_3).DynamiclyGoTo(state3.Id));

        state3.AllOtherTransits()
            .WithReducing((state, transition) =>
            {
                if (transition == TO_STATE_4)                    
                    return new ReductionResult<Transitions, States>(STATE_4).DynamiclyGoTo(state4.Id)
                        .YieldNext(TO_STATE_5)
                        .YieldNext(TO_STATE_6);

                return state;
            });

        state4.TransitsBy(TO_STATE_5)
            .WithReducingTo(STATE_5)
            .To(state5);

        state5.TransitsBy(TO_STATE_6)
            .WithReducingTo(STATE_6)
            .To(state6);

        state6.TransitsBy(TO_ACCEPTED)
            .WithReducingTo(ACCEPTED)
            .ToAccepted();

        return builder.Build().Match(
            value => value,
            _ => throw new Exception("Result is Error."));
    }
}