using BenchmarkDotNet.Attributes;

namespace Starfish.Web.Benchmarks;

[MemoryDiagnoser]
public class StarfishBenchmarks
{
    [Params(100)]
    public int Amount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
    }
    
    
    [Benchmark]
    public async Task DoSomething()
    {
       // Do Something
    }
}