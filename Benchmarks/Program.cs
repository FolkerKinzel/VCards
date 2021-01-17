using System;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal class Program
    {
        private static void Main()
        {
            //_ = BenchmarkRunner.Run<QuotedPrintableBench>();
            _ = BenchmarkRunner.Run<VCardBench>();

        }
    }
}
