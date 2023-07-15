using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards;

namespace Benchmarks;

[MemoryDiagnoser]
public class AnsiFilterBench2
{
    private const string FILE_PATH = @"C:\Users\fkinz\source\repos\FolkerKinzel.VCards\src\Benchmarks\TestFiles\German.vcf";

    [Benchmark(Baseline = true)]
    public string AnsiFilterNew1()
    {
        var filter = new AnsiFilterNew();
        _ = filter.LoadVcf(FILE_PATH, out string enc);
        return enc;
    }

    [Benchmark]
    public string AnsiFilter1()
    {
        var filter = new AnsiFilter();
        _ = filter.LoadVcf(FILE_PATH, out string enc);
        return enc;
    }
}
