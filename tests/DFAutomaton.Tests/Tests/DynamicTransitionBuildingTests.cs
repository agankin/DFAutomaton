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
        state1.TransitsBy(TO2).Dynamicly().WithReducing(Reducer.Create(TO2, STATE2));

        state1[TO2]
            .IsSome()
            .TransitsDynamicly()
            .Reduces(TO2, STATE1, STATE2);
    }
}