using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards.Extensions;
using CommandLine;
using FolkerKinzel.VCards.Enums;

namespace Benchmarks;


[MemoryDiagnoser]
public class HasFlagBench
{
    private readonly Addr? _value = Addr.Dom | Addr.Intl | Addr.Postal;


    [Benchmark]
    public bool IsSet() => _value.IsSet(Addr.Parcel);


    [Benchmark]
    public bool HasFlag() => _value!.Value.HasFlag(Addr.Parcel);
}
