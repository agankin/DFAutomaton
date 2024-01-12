using BenchmarkDotNet.Attributes;
using Optional.Unsafe;

namespace DFAutomaton.Benchmarks;

public class Benchmarks
{
    private const int AddSerieMemberStatesCount = 1000000;
    private Automaton<int, double> _piGregorySeriesAutomaton = null!;

    [GlobalSetup]
    public void Setup()
    {
        _piGregorySeriesAutomaton = PiGregorySeriesAutomaton.Create(AddSerieMemberStatesCount);
    }

    [Benchmark]
    public void RunGregorySeries()
    {
        var transitions = Enumerable.Range(0, AddSerieMemberStatesCount).Append(AddSerieMemberStatesCount);
        var _ = _piGregorySeriesAutomaton.Run(0, transitions).ValueOrFailure();
    }
}
