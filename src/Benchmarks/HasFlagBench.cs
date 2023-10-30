using BenchmarkDotNet.Attributes;
using FolkerKinzel.VCards.Models.Enums;
using FolkerKinzel.VCards.Extensions;
using CommandLine;

namespace Benchmarks;


[MemoryDiagnoser]
public class HasFlagBench
{
    private readonly AddressTypes? _value = (AddressTypes.Dom | AddressTypes.Intl | AddressTypes.Postal);


    [Benchmark]
    public bool IsSet() => _value.IsSet(AddressTypes.Parcel);


    [Benchmark]
    public bool HasFlag() => _value!.Value.HasFlag(AddressTypes.Parcel);
}
