﻿using Optional.Unsafe;

namespace DFAutomaton.Benchmarks
{  
    internal class PiGregorySeriesAutomaton
    {
        public static Automaton<int, double> Create(int serieMembersCount)
        {
            var builder = AutomatonBuilder<int, double>.Create();
            var allMembersAddedState = Enumerable.Range(0, serieMembersCount).Aggregate(builder.Start, ToAddNextSerieMemberState);

            Reduce<int, double> calcPi = (acc, _) => 4 * acc;
            allMembersAddedState.TransitsBy(serieMembersCount).WithReducingBy(calcPi).ToAccepted();

            return builder.Build().ValueOrFailure();
        }

        private static State<int, double> ToAddNextSerieMemberState(State<int, double> state, int idx)
        {
            Reduce<int, double> applyNextSerieMember = (acc, idx) =>
            {
                var sign = idx % 2 == 1 ? -1.0 : 1.0;
                return acc + sign / (2 * idx + 1);
            };
            
            return state.TransitsBy(idx).WithReducingBy(applyNextSerieMember).ToNew();
        }
    }
}