using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards.Intls.Extensions;

namespace Benchmarks;

public class ListConcatBench
{
    private readonly List<ReadOnlyMemory<char>> _shortList = [];

    [Params(1, 2, 3, 4, 5)]
    public int N { get; set; }

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
