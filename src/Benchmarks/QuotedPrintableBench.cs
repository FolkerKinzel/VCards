using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;

namespace Benchmarks
{
    public class QuotedPrintableBench
    {
        [Benchmark]
        public void Decode()
        {
            string quoted = $"1=0D=0AFirmenstra=C3=9Fe=0D=0AOrt Firma, Bundesland Firma PLZFirma=0D=0ALand={Environment.NewLine} Firma";

            _ = QuotedPrintableConverter.Decode(quoted);
        }
    }
}
