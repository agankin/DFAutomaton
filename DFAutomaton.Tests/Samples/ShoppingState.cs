namespace DFAutomaton.Tests
{
    public readonly record struct ShoppingState(ShoppingStateType Type, decimal GoodsCost);

    public enum ShoppingStateType
    {
        Shopping = 1,

        GoodsPaid,

        GoodsPurchased
    }
}