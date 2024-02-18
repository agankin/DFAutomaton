using DFAutomaton.Tests.Samples.Shopping;
using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests;

[TestFixture]
public class AutomatonTests
{
    [Test(Description = "Automaton state transitions test scenario.")]
    public void Run_valid()
    {
        var automaton = StateGraph.BuildAutomaton();

        var wallet = new Wallet(100);
        var initialState = new State(
            Phase.CollectingGoods,
            Cart.Empty,
            wallet
        ).Some<State, Errors>();

        var expectedWalletAfterPurchase = new Wallet(100 - Prices.Bread - Prices.Butter);
        var expectedAfterPurchaseState = new State(
            Phase.GoodsPurchased,
            new Cart(Goods.Bread, Goods.Butter),
            expectedWalletAfterPurchase
        ).Some<State, Errors>();

        var transitions = new[]
        {
            Actions.PutBreadToCart,
            Actions.PutButterToCart,
            Actions.PayForGoods,
            Actions.ReceiveGoods
        };

        var finalState = automaton.Run(initialState, transitions).IsSome();
        Assert.AreEqual(expectedAfterPurchaseState, finalState);
    }

    [Test(Description = "Automaton error occuring on transition not found test scenario.")]
    public void Run_with_transition_not_found()
    {
        var automaton = StateGraph.BuildAutomaton();

        var wallet = new Wallet(100);
        var initialState = new State(
            Phase.CollectingGoods,
            Cart.Empty,
            wallet
        ).Some<State, Errors>();
        
        var transitions = new[]
        {
            Actions.PutBreadToCart,
            Actions.PutButterToCart,
            Actions.ReceiveGoods
        };

        automaton.Run(initialState, transitions)
            .IsError()
            .HasType(AutomatonErrorType.TransitionNotExists)
            .OccuredOn(Actions.ReceiveGoods)
            .State
                .Is(StateType.Start);
    }

    [Test(Description = "Automaton error occuring on transition from accepted state test scenario.")]
    public void Run_with_transit_from_accepted()
    {
        var automaton = StateGraph.BuildAutomaton();

        var wallet = new Wallet(100);
        var initialState = new State(
            Phase.CollectingGoods,
            Cart.Empty,
            wallet
        ).Some<State, Errors>();

        var transitions = new[]
        {
            Actions.PutBreadToCart,
            Actions.PutButterToCart,
            Actions.PayForGoods,
            Actions.ReceiveGoods,

            Actions.PayForGoods
        };

        automaton.Run(initialState, transitions)
            .IsError()
            .HasType(AutomatonErrorType.TransitionFromAccepted)
            .OccuredOn(Actions.PayForGoods)
            .State
                .Is(StateType.Accepted);
    }
}