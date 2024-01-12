using BenchmarkDotNet.Running;
using DFAutomaton.Benchmarks;

BenchmarkRunner.Run<Benchmarks>();

Console.ReadKey(true);