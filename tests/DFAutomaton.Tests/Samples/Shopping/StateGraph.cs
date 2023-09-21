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

        collectingGoods.LinkFixedState(Actions.PutBreadToCart, collectingGoods, Reducers.PutBreadToCart);
        collectingGoods.LinkFixedState(Actions.PutButterToCart, collectingGoods, Reducers.PutButterToCart);
        
        collectingGoods
            .ToNewFixedState(Actions.PayForGoods, Reducers.Pay)
            .ToAccepted(Actions.ReceiveGoods, Reducers.Purchase);

        return builder;
    }
}