using Optional;

namespace DFAutomaton.Tests.Samples.Shopping;

public record State(
    Phase Phase,
    Cart Cart,
    Wallet Wallet
)
{
    public static State Start(decimal moneyAmountInWallet) => new State(
        Phase.CollectingGoods,
        Cart.Empty,
        new Wallet(moneyAmountInWallet)
    );

    public Option<State, Errors> Pay()
    {
        if (Phase != Phase.CollectingGoods)
            return Option.None<State, Errors>(Errors.PayingForNotCollected);

        var result = Wallet.PayFor(Cart);
        return result.Map(wallet => this with { Phase = Phase.GoodsPaid, Wallet = wallet });
    }

    public Option<State, Errors> Purchase()
    {
        if (Phase != Phase.GoodsPaid)
            return Option.None<State, Errors>(Errors.PurchasingNotPaid);

        return (this with { Phase = Phase.GoodsPurchased }).Some<State, Errors>();
    }
}

public enum Phase
{
    CollectingGoods = 1,

    GoodsPaid,

    GoodsPurchased
}