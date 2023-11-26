using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Benchmarks;

[MemoryDiagnoser]
public class EnumerableBench
{
    private readonly string?[] _arr = ["1", null, "2", null, "3", "4", null, "5", "6"];

    private readonly TextProperty?[] _props = [new TextProperty(null), null, new TextProperty("Hi"), null, new TextProperty("Bye bye"), null];

    //[Benchmark]
    //public int NotNullLibrary()
    //{
    //    return _arr.WhereNotNull().Count();
    //}

    //[Benchmark]
    //public int NotNullGenerator()
    //{
    //    return NotNull().Count();
    //}

    [Benchmark]
    public int NotEmptyLibrary()
    {
        return _props.WhereNotEmpty().Count();
    }

    [Benchmark]
    public int NotEmptyGenerator()
    {
        return NotEmpty().Count();
    }

    //[Benchmark]
    //public int NotNullAndLibrary()
    //{
    //    return _props.WhereNotNullAnd(static x => x.Value != "Hi").Count();
    //}

    //[Benchmark]
    //public int NotNullAndGenerator()
    //{
    //    return NotNullAnd(static x => x.Value != "Hi").Count();
    //}

    private IEnumerable<string> NotNull()
    {
        foreach (var item in _arr)
        {
            if (item != null)
            {
                yield return item;
            }
        }
    }

    private IEnumerable<TextProperty> NotEmpty()
    {
        foreach (TextProperty? item in _props)
        {
            if (item != null && !item.IsEmpty)
            {
                yield return item;
            }
        }
    }

    private IEnumerable<TextProperty> NotNullAnd(Func<TextProperty, bool> func)
    {
        foreach (TextProperty? item in _props)
        {
            if (item != null && func(item))
            {
                yield return item;
            }
        }
    }
}
