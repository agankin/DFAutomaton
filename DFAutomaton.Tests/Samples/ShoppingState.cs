namespace DFAutomaton.Tests
{
    public readonly record struct ShoppingState(
        ShoppingStateType Type,
        decimal GoodsCost);
}