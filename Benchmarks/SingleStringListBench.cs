using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards.Models;

namespace Benchmarks
{
    public class SingleStringListBench
    {
        [Benchmark]
        public void StructVsClass()
        {
            _ = new AddressProperty("eins", "zwei", "drei", "vier", "fünf", "sechs", "sieben");
        }

    }
}
