namespace DFAutomaton.Tests;

using static Transitions;
using static States;

internal static class TestAutomaton
{
    public static Automaton<Transitions, States> Create()
    {
        var builder = new AutomatonBuilder<Transitions, States>();
        
        var state1 = builder.Start;
        var state3 = builder.CreateState();
        var state4 = builder.CreateState();
        var state5 = builder.CreateState();
        var state6 = builder.CreateState();

        var state2 = state1
            .ForInput(TO_STATE_1)
                .Reduce(STATE_1)
                .GoToSelf()
            .WhenInput(transition => transition == TO_STATE_2)
                .Reduce((_, _) => STATE_2)
                .GoToNewState();

        state2.ForInput(TO_STATE_3)
            .Reduce((_, _, emit) =>
            {
                emit(new GoToState<Transitions, States>(state3.Id));
                return STATE_3;
            });

        state3.ForAllOtherInputs()
            .Reduce((state, transition, emit) =>
            {
                if (transition == TO_STATE_4) 
                {
                    emit(new YieldNext<Transitions, States>(TO_STATE_5));
                    emit(new YieldNext<Transitions, States>(TO_STATE_6));
                    emit(new GoToState<Transitions, States>(state4.Id));

                    return STATE_4;
                }

                return state;
            });

        state4.ForInput(TO_STATE_5)
            .Reduce(STATE_5)
            .GoTo(state5);

        state5.ForInput(TO_STATE_6)
            .Reduce(STATE_6)
            .GoTo(state6);

        state6.ForInput(TO_ACCEPTED)
            .Reduce(ACCEPTED)
            .GoToAccepted();

        return builder.Build().Match(
            value => value,
            _ => throw new Exception("Result is Error."));
    }
}