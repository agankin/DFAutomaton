﻿using Optional.Unsafe;

namespace DFAutomaton.Benchmarks
{
    internal class PiGregorySeriesAutomaton
    {
        public static Automaton<int, double> Create(int serieMembersCount)
        {
            var builder = AutomatonBuilder<int, double>.Create();
            var allMembersAddedState = Enumerable.Range(0, serieMembersCount).Aggregate(builder.Start, ToAddNextSerieMemberState);

            ReduceValue<int, double> calcPi = automatonState => 4 * automatonState.CurrentValue;
            var toPiNumberState = allMembersAddedState.ToAccepted(serieMembersCount, calcPi);

            return builder.Build().ValueOrFailure();
        }

        private static State<int, double> ToAddNextSerieMemberState(State<int, double> state, int idx)
        {
            var sign = idx % 2 == 1 ? -1.0 : 1.0;
            ReduceValue<int, double> applyNextSerieMember = automatonState => automatonState.CurrentValue + sign / (2 * idx + 1);
            
            return state.ToNewFixedState(idx, applyNextSerieMember);
        }
    }
}