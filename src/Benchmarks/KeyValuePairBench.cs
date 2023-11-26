using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

public class KeyValuePairBench
{
    private readonly List<(string Key, string Value)> _tupleList = [];

    public List<(string Key, string Value)> TupleList => _tupleList;



    [Benchmark]
    public void KeyValuePair()
    {
        var KvpList = new List<KeyValuePair<string, string>>();


        var kvp = new KeyValuePair<string, string>("1", "2");

        KvpList.Add(kvp);
        _ = KvpList[0];

        kvp = new KeyValuePair<string, string>("3", "4");

        KvpList.Add(kvp);
        _ = KvpList[0];


        kvp = new KeyValuePair<string, string>("5", "6");

        KvpList.Add(kvp);
        _ = KvpList[0];


        kvp = new KeyValuePair<string, string>("7", "8");

        KvpList.Add(kvp);
        _ = KvpList[0];

    }

    [Benchmark]
    public void Tuple()
    {
        var KvpList = new List<(string Key, string Value)>();

        (string Key, string Value) kvp = (Key: "1", Value: "2");

        KvpList.Add(kvp);
        _ = KvpList[0];


        kvp = (Key: "3", Value: "4");

        KvpList.Add(kvp);
        _ = KvpList[0];


        kvp = (Key: "5", Value: "6");

        KvpList.Add(kvp);
        _ = KvpList[0];


        kvp = (Key: "7", Value: "8");

        KvpList.Add(kvp);
        _ = KvpList[0];

    }

}
