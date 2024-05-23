// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Benchmarks;

//_ = BenchmarkRunner.Run<ParseBench>();
_ = BenchmarkRunner.Run<WriteBench>();
