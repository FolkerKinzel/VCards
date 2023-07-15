using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards;

namespace Benchmarks;

[MemoryDiagnoser]
public class AnsiFilterBench
{
    private const string FILE_PATH = "C:\\Users\\fkinz\\source\\repos\\FolkerKinzel.VCards\\src\\Benchmarks\\TestFiles\\Ukrainian.vcf";

    [Benchmark(Baseline = true)]
    public string AnsiFilterNew1()
    {
        var filter = new AnsiFilterNew();
        _ = filter.LoadVcf(FILE_PATH, out string enc);
        return enc;
    }

    [Benchmark]
    public string MultiAnsiFilter1()
    {
        var filter = new MultiAnsiFilter();
        _ = filter.LoadVcf(FILE_PATH, out string enc);
        return enc;
    }
}
