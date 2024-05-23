using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Extensions;

namespace Benchmarks;

[MemoryDiagnoser]
public class ConcatWithBench
{
    private const int COUNT = 4;
    private readonly TextProperty _prop = new("Hi");
    //private readonly IEnumerable<TextProperty?> _concat1;
    //private readonly IEnumerable<TextProperty?> _concat2;

    //public ConcatWithBench()
    //{
    //    _concat1 = ConcatWithTest()!;
    //    _concat2 = ConcatWith2Test()!;

    //}

    [Benchmark]
    public int ConcatWith() => ConcatWithTest()!.Count();

    [Benchmark]
    public int ConcatWith2() => ConcatWith2Test()!.Count();

    public IEnumerable<TextProperty?>? ConcatWithTest()
    {
        IEnumerable<TextProperty?>? numerable = null;

        for (int i = 0; i < COUNT; i++) 
        {
            numerable = numerable.ConcatWith(_prop);
        }

        return numerable;
    }


    public IEnumerable<TextProperty?>? ConcatWith2Test()
    {
        IEnumerable<TextProperty?>? numerable = null;

        for (int i = 0; i < COUNT; i++)
        {
            numerable = numerable.ConcatWith(_prop);
        }

        return numerable;
    }

    //[Benchmark]
    //public int ReturnsTest()
    //{
    //    int i = 0;

    //    foreach (var item in _concat1)
    //    {
    //        i++;
    //    }

    //    return i;
    //}

    //[Benchmark]
    //public int Returns2Test()
    //{
    //    int i = 0;

    //    foreach (var item in _concat2)
    //    {
    //        i++;
    //    }

    //    return i;
    //}
}
