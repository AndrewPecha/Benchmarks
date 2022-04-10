// See https://aka.ms/new-console-template for more information

using AutoBogus;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

Console.WriteLine("Hello, World!");
var summary = BenchmarkRunner.Run(typeof(Program).Assembly);

public class EnumerableBenchmarks
{
    private readonly IEnumerable<string> _vanillaEnumerable;
    private readonly IAsyncEnumerable<string> _asyncEnumerable;
    
    public EnumerableBenchmarks()
    {
        var data = new AutoFaker<string>().Generate(1000);
        _vanillaEnumerable = data.AsEnumerable();
        _asyncEnumerable = AsAsyncEnumerable(data);
    }

    private async IAsyncEnumerable<string> AsAsyncEnumerable(IEnumerable<string> enumerable)
    {
        foreach (var item in enumerable)
        {
            yield return item;
        }
    }

    [Benchmark]
    public void IterateVanilla()
    {
        foreach (var item in _vanillaEnumerable)
        {
            //do nothing, just iterating
        }
    }
    
    [Benchmark]
    public async Task IterateAsync()
    {
        await foreach (var item in _asyncEnumerable)
        {
            //do nothing, just iterating
        }
    }
}