using DFAutomaton.Tests.Samples.Shopping;
using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests;

[TestFixture]
public class StateGraphTests
{
    [Test(Description = "State graph transitions test scenario.")]
    public void Transitions_scenario()
    {
        var collectingGoods = StateGraph.BeginCollectingGoods();
        
        var emptyCartState = new State(
            Phase.CollectingGoods,
            Cart.Empty,
            new Wallet(100)
        ).Some<State, Errors>();
        
        var breadInCartState = new State(
            Phase.CollectingGoods,
            new Cart(Goods.Bread),
            new Wallet(100)
        ).Some<State, Errors>();
        
        var breadInCart = collectingGoods[Actions.PutBreadToCart]
            .IsSome()
            .TransitsTo(collectingGoods)
            .Reduces(Actions.PutBreadToCart, emptyCartState, breadInCartState)
            .ToState
                .IsSome()
                .Is(StateType.Start);

        var breadButterInCartState = new State(
            Phase.CollectingGoods,
            new Cart(Goods.Bread, Goods.Butter),
            new Wallet(100)
        ).Some<State, Errors>();

        var breadButterInCart = breadInCart[Actions.PutButterToCart]
            .IsSome()
            .TransitsTo(collectingGoods)
            .Reduces(Actions.PutButterToCart, breadInCartState, breadButterInCartState)
            .ToState
                .IsSome()
                .Is(StateType.Start);

        var walletAfterPayment = new Wallet(100 - Prices.Bread - Prices.Butter);
        var paidState = new State(
            Phase.GoodsPaid,
            new Cart(Goods.Bread, Goods.Butter),
            walletAfterPayment
        ).Some<State, Errors>();

        var paid = breadButterInCart[Actions.PayForGoods]
            .IsSome()
            .Reduces(Actions.PayForGoods, breadButterInCartState, paidState)
            .ToState
                .IsSome()
                .Is(StateType.SubState);

        var purchasedState = new State(
            Phase.GoodsPurchased,
            new Cart(Goods.Bread, Goods.Butter),
            walletAfterPayment
        ).Some<State, Errors>();

        paid[Actions.ReceiveGoods]
            .IsSome()
            .Reduces(Actions.ReceiveGoods, paidState, purchasedState)
            .ToState
                .IsSome()
                .Is(StateType.Accepted);
    }

    [Test(Description = "Tests transition not exists.")]
    public void Transition_not_exists()
    {
        var collectingGoods = StateGraph.BeginCollectingGoods();
        collectingGoods[Actions.ReceiveGoods].IsNone();
    }
}