// See https://aka.ms/new-console-template for more information

using System.Collections.Frozen;
using BenchmarkDotNet.Running;
using Experiments;
using FolkerKinzel.VCards.Models;

//var bench = new FrozenDictionaryBench();

//var frozen = Enumerable.Range(1,2).ToFrozenDictionary(x => x);

//BenchmarkDotNet.Reports.Summary report = BenchmarkRunner.Run<FrozenDictionaryBench>();
BenchmarkDotNet.Reports.Summary report = BenchmarkRunner.Run<ConcatBench>();


Console.WriteLine(report);