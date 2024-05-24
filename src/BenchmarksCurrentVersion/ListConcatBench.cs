using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using FolkerKinzel.VCards.Intls.Extensions;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net48)]
public class ListConcatBench
{
    private readonly List<ReadOnlyMemory<char>> _shortList = new(); 

    [Params(1,2,3,4,5)]
    public int N {  get; set; }

    public void GlobalSetup()
    {
        _shortList.Clear();

        for (int i = 0; i < N; i++)
        {
            _shortList.Add(ReadOnlyMemory<char>.Empty);
        }
    }

    [Benchmark]
    public int ConcatShortListBench() => _shortList.Concat().Length;
}
