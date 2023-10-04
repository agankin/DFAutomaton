using DFAutomaton.Tests.Samples.Shopping;
using NUnit.Framework;
using Optional;

namespace DFAutomaton.Tests;

[TestFixture]
public class AutomatonStateGraphTests
{
    [Test(Description = "Automaton state graph transitions test scenario.")]
    public void Transitions_scenario()
    {
        var builder = StateGraph.BuildAutomaton();
        var collectingGoods = builder.Start;
        
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
            .TransitsTo(TransitionKind.FixedState)
            .TransitsTo(collectingGoods)
            .Reduces(emptyCartState, breadInCartState)
            .State
                .IsSome()
                .Is(StateType.Start);

        var breadButterInCartState = new State(
            Phase.CollectingGoods,
            new Cart(Goods.Bread, Goods.Butter),
            new Wallet(100)
        ).Some<State, Errors>();

        var breadButterInCart = breadInCart[Actions.PutButterToCart]
            .IsSome()
            .TransitsTo(TransitionKind.FixedState)
            .TransitsTo(collectingGoods)
            .Reduces(breadInCartState, breadButterInCartState)
            .State
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
            .TransitsTo(TransitionKind.FixedState)
            .Reduces(breadButterInCartState, paidState)
            .State
                .IsSome()
                .Is(StateType.SubState);

        var purchasedState = new State(
            Phase.GoodsPurchased,
            new Cart(Goods.Bread, Goods.Butter),
            walletAfterPayment
        ).Some<State, Errors>();

        paid[Actions.ReceiveGoods]
            .IsSome()
            .TransitsTo(TransitionKind.FixedState)
            .Reduces(paidState, purchasedState)
            .State
                .IsSome()
                .Is(StateType.Accepted);
    }

    [Test(Description = "Tests automaton transition not exists.")]
    public void Transition_not_exists()
    {
        var builder = StateGraph.BuildAutomaton();
        var collectingGoods = builder.Start;
        
        collectingGoods[Actions.ReceiveGoods].IsNone();
    }
}