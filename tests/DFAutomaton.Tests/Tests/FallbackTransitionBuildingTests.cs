using NUnit.Framework;

namespace DFAutomaton.Tests;

using static Transitions;
using static States;

[TestFixture(Description = "Tests fallback transition building.")]
public partial class FallbackTransitionBuildingTests
{
    [Test(Description = "Tests adding a fallback transition.")]
    public void Add_dynamic_transition()
    {
        var state1 = new AutomatonBuilder<Transitions, States>().Start;
        state1.AllOtherTransits().WithReducing(Reducer.Create(TO_STATE_2, STATE_2));

        state1[TO_STATE_2]
            .IsSome()
            .TransitsDynamicly()
            .Reduces(TO_STATE_2, STATE_1, STATE_2);
    }
}