using NUnit.Framework;

namespace DFAutomaton.Tests;

using static Transitions;
using static States;

[TestFixture(Description = "Tests dynamic transition building.")]
public partial class FixedTransitionBuildingTests
{
    [Test(Description = "Tests adding a dynamic transition.")]
    public void Add_dynamic_transition()
    {
        var state1 = new StateGraph<Transitions, States>().StartState;
        state1.TransitsBy(TO_STATE_2).Dynamicly().WithReducing(Reducer.Create(TO_STATE_2, STATE_2));

        state1[TO_STATE_2]
            .IsSome()
            .TransitsDynamicly()
            .Reduces(TO_STATE_2, STATE_1, STATE_2);
    }
}