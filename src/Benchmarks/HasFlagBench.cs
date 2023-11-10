using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards.Extensions;
using CommandLine;
using FolkerKinzel.VCards.Enums;

namespace Benchmarks;


[MemoryDiagnoser]
public class HasFlagBench
{
    private readonly Adr? _value = (Adr.Dom | Adr.Intl | Adr.Postal);


    [Benchmark]
    public bool IsSet() => _value.IsSet(Adr.Parcel);


    [Benchmark]
    public bool HasFlag() => _value!.Value.HasFlag(Adr.Parcel);
}
