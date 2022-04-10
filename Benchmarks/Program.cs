// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoBogus;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}

public class EnumerableBenchmarks
{
    private readonly IEnumerable<string> _vanillaEnumerable;
    private readonly IAsyncEnumerable<string> _asyncEnumerable;
    
    public EnumerableBenchmarks()
    {
        var data = new AutoFaker<string>().Generate(1000);
        data[50] = "foo";
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

    [Benchmark]
    public void AnyVanilla()
    {
        var foo = _vanillaEnumerable.Any(x => x == "foo");
    }
    
    [Benchmark]
    public async Task AnyAsync()
    {
        var foo = await _asyncEnumerable.AnyAsync(x => x == "foo");
    }
}