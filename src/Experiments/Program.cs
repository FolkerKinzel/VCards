// See https://aka.ms/new-console-template for more information

using System.Collections.Frozen;
using BenchmarkDotNet.Running;
using Experiments;

//var bench = new FrozenDictionaryBench();

//var frozen = Enumerable.Range(1,2).ToFrozenDictionary(x => x);

var report = BenchmarkRunner.Run<FrozenDictionaryBench>();

Console.WriteLine(report);
