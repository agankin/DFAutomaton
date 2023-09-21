using DFAutomaton.Tests.Samples.Shopping;
using Optional;

namespace DFAutomaton.Tests;

public readonly record struct Wallet(decimal MoneyAmount)
{
    public Option<Wallet, Errors> PayFor(Cart cart)
    {
        var goods = cart.Goods;
        if (!goods.Any())
            return Option.None<Wallet, Errors>(Errors.NoGoodsToPay);

        var totalCost = goods.Sum(good => good.GetPrice());
        return MoneyAmount >= totalCost
            ? (this with { MoneyAmount = MoneyAmount - totalCost }).Some<Wallet, Errors>()
            : Option.None<Wallet, Errors>(Errors.InsufficientFunds);
    }
}