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
            .TransitsBy(Actions.PutBreadToCart).WithReducingBy(Reducers.PutBreadToCart).ToSelf()
            .TransitsBy(Actions.PutButterToCart).WithReducingBy(Reducers.PutButterToCart).ToSelf()
            .TransitsBy(Actions.PayForGoods).WithReducingBy(Reducers.Pay).ToNew()
            .TransitsBy(Actions.ReceiveGoods).WithReducingBy(Reducers.Purchase).ToAccepted();

        return builder;
    }
}