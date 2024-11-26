using NUnit.Framework;

namespace DFAutomaton.Tests;

using static Transitions;
using static States;

[TestFixture(Description = "Tests dynamic transition building.")]
public partial class DynamicTransitionBuildingTests
{
    [Test(Description = "Tests adding a dynamic transition.")]
    public void Add_dynamic_transition()
    {
        var state1 = new AutomatonBuilder<Transitions, States>().Start;
        state1.TransitsBy(TO_STATE_2).Dynamicly().WithReducingBy(Reducer.Create(TO_STATE_2, STATE_2));

        state1[TO_STATE_2]
            .IsSome()
            .TransitsDynamicly()
            .Reduces(TO_STATE_2, STATE_1, STATE_2);
    }
}