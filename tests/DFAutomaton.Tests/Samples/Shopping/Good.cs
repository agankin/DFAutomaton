namespace DFAutomaton.Tests.Samples.Shopping;

public enum Goods
{
    Bread = 1,

    Butter
}

public static class Prices
{
    public const decimal Bread = 5.00m;

    public const decimal Butter = 10.00m;
}

public static class GoodExtensions
{
    public static decimal GetPrice(this Goods good) =>
        good switch
        {
            Goods.Bread => Prices.Bread,
            Goods.Butter => Prices.Butter,
            _ => throw new Exception($"Unsupported {nameof(Goods)} enum value: {good}.")
        };
}