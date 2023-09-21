using System.Collections.Immutable;

namespace DFAutomaton.Tests.Samples.Shopping;

public record Cart(ImmutableList<Goods> Goods)
{
    public Cart(params Goods[] goods) : this(goods.Aggregate(ImmutableList<Goods>.Empty, (list, good) => list.Add(good)))
    {
    }

    public static readonly Cart Empty = new Cart(ImmutableList<Goods>.Empty);
    
    public Cart Put(Goods good) => this with { Goods = Goods.Add(good) };

    public Cart Remove(Goods good) => this with { Goods = Goods.Remove(good) };

    public decimal GetTotalCost() => Goods.Sum(good => good.GetPrice());
}