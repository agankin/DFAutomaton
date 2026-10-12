namespace DFAutomaton.Benchmarks
{  
    internal class PiGregorySeriesAutomaton
    {
        public static Automaton<int, double> Create(int serieMembersCount)
        {
            var builder = new AutomatonBuilder<int, double>();
            var allMembersAddedState = Enumerable.Range(0, serieMembersCount).Aggregate(builder.Start, ToAddNextSerieMemberState);

            PlainReduce<int, double> calcPi = (acc, _) => 4 * acc;
            allMembersAddedState.ForInput(serieMembersCount).Reduce(calcPi).GoToAccepted();

            return builder.Build().Match(
                value => value,
                _ => throw new Exception("Result is Error."));
        }

        private static State<int, double> ToAddNextSerieMemberState(State<int, double> state, int idx)
        {
            PlainReduce<int, double> applyNextSerieMember = (acc, idx) =>
            {
                var sign = idx % 2 == 1 ? -1.0 : 1.0;
                return acc + sign / (2 * idx + 1);
            };
            
            return state.ForInput(idx).Reduce(applyNextSerieMember).GoToNewState();
        }
    }
}