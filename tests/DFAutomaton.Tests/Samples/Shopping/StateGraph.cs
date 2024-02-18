using Optional;
using Optional.Unsafe;

namespace DFAutomaton.Tests.Samples.Shopping;

using StateOrError = Option<State, Errors>;

public class StateGraph
{
    public static Automaton<Actions, StateOrError> BuildAutomaton() => GetBuilder().Build().ValueOrFailure();

    public static State<Actions, StateOrError> BeginCollectingGoods() => GetBuilder().Start;

    private static AutomatonBuilder<Actions, StateOrError> GetBuilder()
    {
        var builder = AutomatonBuilder<Actions, StateOrError>.Create();
        var collectingGoods = builder.Start;

        collectingGoods
            .TransitsBy(Actions.PutBreadToCart).WithReducing(Reducers.PutBreadToCart).ToSelf()
            .TransitsBy(Actions.PutButterToCart).WithReducing(Reducers.PutButterToCart).ToSelf()
            .TransitsBy(Actions.PayForGoods).WithReducing(Reducers.Pay).ToNew()
            .TransitsBy(Actions.ReceiveGoods).WithReducing(Reducers.Purchase).ToAccepted();

        return builder;
    }
}