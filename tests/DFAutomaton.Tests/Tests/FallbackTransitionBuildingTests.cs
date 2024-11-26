using NUnit.Framework;

namespace DFAutomaton.Tests;

using static Transitions;
using static States;

[TestFixture(Description = "Tests fallback transition building.")]
public partial class FallbackTransitionBuildingTests
{
    [Test(Description = "Tests adding a fixed fallback transition.")]
    public void Add_fixed_fallback_transition()
    {
        var state1 = new AutomatonBuilder<Transitions, States>().Start;
        state1.AllOtherTransits().WithReducingBy(Reducer.Create(TO_STATE_2, STATE_2)).ToSelf();

        state1[TO_STATE_2]
            .IsSome()
            .TransitsTo(state1)
            .Reduces(TO_STATE_2, STATE_1, STATE_2);
    }
    
    [Test(Description = "Tests adding a dynamic fallback transition.")]
    public void Add_dynamic_fallback_transition()
    {
        var state1 = new AutomatonBuilder<Transitions, States>().Start;
        state1.AllOtherTransits().Dynamicly().WithReducingBy(Reducer.Create(TO_STATE_2, STATE_2));

        state1[TO_STATE_2]
            .IsSome()
            .TransitsDynamicly()
            .Reduces(TO_STATE_2, STATE_1, STATE_2);
    }
}