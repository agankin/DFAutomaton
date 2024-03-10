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
        var state1 = new StateGraph<Transitions, States>().StartState;
        state1.AllOtherTransits().WithReducing(Reducer.Create(TO2, STATE2));

        state1[TO2]
            .IsSome()
            .TransitsDynamicly()
            .Reduces(TO2, STATE1, STATE2);
    }
}