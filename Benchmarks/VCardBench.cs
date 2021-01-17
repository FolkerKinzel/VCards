using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards;

namespace Benchmarks
{
    public class VCardBench
    {
        [Benchmark]
        public void ParseLongVcf()
        {
            _ = VCard.Load(@"C:\Users\fkinz\OneDrive\Kontakte\Thunderbird\21-01-13.vcf");
        }
    }
}
